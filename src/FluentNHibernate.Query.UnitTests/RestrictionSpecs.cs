using FluentNHibernate.Query.UnitTests.Model;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SpecUnit;

namespace FluentNHibernate.Query.UnitTests
{
	public class RestrictionSpecs
	{

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_one_restriction : ContextSpecification
		{
			private A result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetOne<A>().Where(a => a.ABC).IsEqualTo("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction()
			{
				criteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_one_restriction_and_no_results : ContextSpecification
		{
			private A result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new A[0]);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetOne<A>().Where(a => a.ABC).IsEqualTo("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_no_result()
			{
				result.ShouldBeNull();
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction()
			{
				criteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_multiple_restriction : ContextSpecification
		{
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.Where(a => a.DEF).IsLessThan(3)
					.And(a => a.DEF).IsGreaterThan(0)
					.And(a => a.ABC).IsNull()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_three_restrictions()
			{
				criteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Times(3));
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_a_IsLike_restriction : ContextSpecification
		{
			private A result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetOne<A>().Where(a => a.ABC).IsLike("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_add_the_islike_restriction()
			{
				criteria.AssertWasCalled(c => c.Add(Restrictions.Like("ABC", "abc")), mo => mo
					.Constraints(Is.TypeOf<SimpleExpression>())
					.Repeat.Once()
					);
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_an_IsLikeInsensitive_restriction : ContextSpecification
		{
			private A result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetOne<A>().Where(a => a.ABC).IsLikeInsensitive("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_add_the_islike_restriction()
			{
				criteria.AssertWasCalled(c => c.Add(Restrictions.InsensitiveLike("ABC", "abc")), mo => mo
					.Constraints(Is.TypeOf<AbstractCriterion>())
					.Repeat.Once()
					);
			}
		}

	}
}
