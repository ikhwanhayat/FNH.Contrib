using System;
using System.Collections.Generic;
using FluentNHibernate.Query.UnitTests.Model;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using SpecUnit;

namespace FluentNHibernate.Query.UnitTests
{

	public class FluentNHibernateQueries
	{

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_no_additional_restrictions : ContextSpecification
		{
			private A result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] {new A()});

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof (A))).Return(criteria);

				result = session.GetOne<A>().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_not_add_any_restrictions()
			{
				criteria.AssertWasNotCalled(c => c.Add(null), o => o.IgnoreArguments());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_no_additional_restrictions_and_no_results : ContextSpecification
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

				result = session.GetOne<A>().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_no_result()
			{
				result.ShouldBeNull();
			}

			[Test]
			[Observation]
			public void Should_not_add_any_restrictions()
			{
				criteria.AssertWasNotCalled(c => c.Add(null), o => o.IgnoreArguments());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_criteria_on_a_child_object : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;

			protected override void Context()
			{
				childCriteria = MockRepository.GenerateMock<ICriteria>();

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChild(a => a.B)
					.Where(b => b.GHI).IsEqualTo(true)
					.And(b => b.JKL).IsIn("yo momma")
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_restrictions_and_criteria_on_a_child_object : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;

			protected override void Context()
			{
				childCriteria = MockRepository.GenerateMock<ICriteria>();

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.Where(a => a.ABC).IsEqualTo("abc")
					.AndHasChild(a => a.B)
					.Where(b => b.GHI).IsEqualTo(true)
					.And(b => b.JKL).IsGreaterThan('5')
					.EndChild()
					.And(a => a.DEF).IsNotNull()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions()
			{
				criteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_with_criteria_on_a_child_collection : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;

			protected override void Context()
			{
				childCriteria = MockRepository.GenerateMock<ICriteria>();

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("BList")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChildCollection(a => a.BList)
					.Where(b => b.JKL).IsLessThanOrEqualTo('b')
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_restrictions_and_criteria_on_a_child_collection : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;

			protected override void Context()
			{
				childCriteria = MockRepository.GenerateMock<ICriteria>();

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("BList")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.Where(a => a.ABC).IsEqualTo("abc")
					.AndHasChildCollection(a => a.BList)
					.Where(b => b.JKL).IsEqualTo('2')
					.EndChild()
					.And(a => a.DEF).IsNotNull()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions()
			{
				criteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_two_levels_of_criteria_on_a_child_objects : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;
			private ICriteria secondChildCriteria;

			protected override void Context()
			{
				secondChildCriteria = MockRepository.GenerateMock<ICriteria>();

				childCriteria = MockRepository.GenerateMock<ICriteria>();
				childCriteria.Expect(c => c.CreateCriteria("C")).Return(secondChildCriteria);

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChild(a => a.B)
					.ThatHasChild(b => b.C)
					.Where(c => c.MNO).IsGreaterThanOrEqualTo(.55)
					.EndChild()
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction_to_the_child_criteria()
			{
				secondChildCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_restrictions_and_two_levels_of_criteria_on_a_child_objects : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;
			private ICriteria secondChildCriteria;

			protected override void Context()
			{
				secondChildCriteria = MockRepository.GenerateMock<ICriteria>();

				childCriteria = MockRepository.GenerateMock<ICriteria>();
				childCriteria.Expect(c => c.CreateCriteria("C")).Return(secondChildCriteria);

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChild(a => a.B)
					.Where(b => b.GHI).IsEqualTo(false)
					.AndHasChild(b => b.C)
					.Where(c => c.MNO).IsGreaterThanOrEqualTo(.55)
					.And(c => c.PQR).IsIn(new DateTime?[] { DateTime.Now, DateTime.Today })
					.EndChild()
					.And(b => b.JKL).IsGreaterThanOrEqualTo('X')
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_second_child_criteria()
			{
				secondChildCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_where_a_child_has_a_child_collection : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;
			private ICriteria secondChildCriteria;

			protected override void Context()
			{
				secondChildCriteria = MockRepository.GenerateMock<ICriteria>();

				childCriteria = MockRepository.GenerateMock<ICriteria>();
				childCriteria.Expect(c => c.CreateCriteria("CList")).Return(secondChildCriteria);

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChild(a => a.B)
					.ThatHasChildCollection(b => b.CList)
					.Where(c => c.MNO).IsNotEqualTo(Math.PI)
					.EndChild()
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_one_restriction_to_the_child_criteria()
			{
				secondChildCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_where_a_child_with_restrictions_has_a_child_collection : ContextSpecification
		{
			private ICriteria criteria;
			private ICriteria childCriteria;
			private ICriteria secondChildCriteria;

			protected override void Context()
			{
				secondChildCriteria = MockRepository.GenerateMock<ICriteria>();

				childCriteria = MockRepository.GenerateMock<ICriteria>();
				childCriteria.Expect(c => c.CreateCriteria("CList")).Return(secondChildCriteria);

				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });
				criteria.Expect(c => c.CreateCriteria("B")).Return(childCriteria);

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				session.GetOne<A>()
					.ThatHasChild(a => a.B)
					.Where(b => b.GHI).IsNotNull()
					.AndHasChildCollection(b => b.CList)
					.Where(c => c.MNO).IsNotEqualTo(Math.PI)
					.And(c => c.PQR).IsLessThan(DateTime.Today.Subtract(new TimeSpan(5, 0, 0, 0)))
					.EndChild()
					.And(b => b.JKL).IsNotEqualTo('Q')
					.EndChild()
					.Execute();
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_child_criteria()
			{
				childCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}

			[Test]
			[Observation]
			public void Should_add_two_restrictions_to_the_second_child_criteria()
			{
				secondChildCriteria.AssertWasCalled(c => c.Add(null), o => o.IgnoreArguments().Repeat.Twice());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_all_with_no_additional_restrictions : ContextSpecification
		{
			private IList<A> results;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] {new A(), new A()});

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				results = session.GetAll<A>().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_some_results()
			{
				results.Count.ShouldEqual(2);
			}

			[Test]
			[Observation]
			public void Should_not_add_any_restrictions()
			{
				criteria.AssertWasNotCalled(c => c.Add(null), o => o.IgnoreArguments());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_all_with_one_restriction : ContextSpecification
		{
			private IList<A> results;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] {new A(), new A()});

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				results = session.GetAll<A>().Where(a => a.ABC).IsEqualTo("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_some_results()
			{
				results.Count.ShouldEqual(2);
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
		public class When_getting_the_count_with_criteria : ContextSpecification
		{
			private int result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetProjection(null)).IgnoreArguments().Return(criteria);
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<int>()).Return(new[] {8});

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetCount<A>().Where(a => a.ABC).IsEqualTo("abc").Execute();
			}

			[Test]
			[Observation]
			public void Should_return_the_number_of_results()
			{
				result.ShouldEqual(8);
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
		public class When_getting_the_count_with_no_criteria : ContextSpecification
		{
			private int result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetProjection(null)).IgnoreArguments().Return(criteria);
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<int>()).Return(new[] {8});

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetCount<A>().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_the_number_of_results()
			{
				result.ShouldEqual(8);
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_one_with_a_specific_child : ContextSpecification
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

				result = session.GetOne<A>().ThatHasChild(a => a.B).EndChild().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_create_the_child_criteria()
			{
				criteria.AssertWasCalled(c => c.CreateCriteria(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

		[TestFixture]
		[Concern("Creating an NHibernate Query")]
		public class When_getting_all_with_a_specific_child : ContextSpecification
		{
			private IList<A> result;
			private ICriteria criteria;

			protected override void Context()
			{
				criteria = MockRepository.GenerateMock<ICriteria>();
				criteria.Expect(c => c.SetMaxResults(1)).Return(criteria);
				criteria.Expect(c => c.List<A>()).Return(new[] { new A() });

				ISession session = MockRepository.GenerateMock<ISession>();
				session.Expect(s => s.CreateCriteria(typeof(A))).Return(criteria);

				result = session.GetAll<A>().ThatHasChild(a => a.B).EndChild().Execute();
			}

			[Test]
			[Observation]
			public void Should_return_a_result()
			{
				result.ShouldNotBeNull();
			}

			[Test]
			[Observation]
			public void Should_create_the_child_criteria()
			{
				criteria.AssertWasCalled(c => c.CreateCriteria(null), o => o.IgnoreArguments().Repeat.Once());
			}
		}

	}

}