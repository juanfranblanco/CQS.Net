using System;
using System.Linq;
using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Repositories;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.Repository.SqlGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQS.Demo.Business.Tests
{
   
    [TestClass]
    public class RiskRepositoryIntegrationTest
    {
        private GenericRepository<Risk> CreateNewRiskRepository()
        {
            var entityMetadata = new EntityMetadata<Risk>();
            return new GenericRepository<Risk>(
                                        new SqlDeleteGenerator<Risk>(entityMetadata),
                                        new SqlInsertGenerator<Risk>(entityMetadata),
                                        new SqlSelectGenerator<Risk>(entityMetadata, new SqlWhereGenerator<Risk>(entityMetadata)),
                                        new SqlUpdateGenerator<Risk>(entityMetadata), entityMetadata,
                                        new DbConnectionFactory());
        }

        [TestMethod]
        public void Should_Insert_And_Update_UnitOfWork()
        {
            var unitOfWork = new UnitOfWork();

            //Given
            var risk = new Risk();
            risk.Address1 = "Crescent";
            risk.Address2 = "Tyne & Wear";
            risk.Name = "Home";
            risk.PostCode = "NE30";

            //When

            var repository = CreateNewRiskRepository();

            unitOfWork.BeginTransaction();

            var inserted = repository.Insert(risk);

            Assert.IsTrue(inserted);

            Assert.IsTrue(risk.RiskId > 0);

            risk.Address1 = "Park";

            repository.Update(risk);

            unitOfWork.CommitTransaction();

            //Then

            var riskRefreshed = repository.GetFirst(new {RiskId = risk.RiskId});

            Assert.AreEqual(risk.Address1, riskRefreshed.Address1);

        }


        [TestMethod]
        public void Should_Rollback_UnitOfWork()
        {
            var unitOfWork = new UnitOfWork();

            //Given
            var risk = new Risk();
            risk.Address1 = "Crescent";
            risk.Address2 = "Tyne & Wear";
            risk.Name = "Home";
            risk.PostCode = "NE30";

            //When

            var repository = CreateNewRiskRepository();

            unitOfWork.BeginTransaction();

            var inserted = repository.Insert(risk);

            //Then

            Assert.IsTrue(inserted);
            Assert.IsTrue(risk.RiskId > 0);

            unitOfWork.RollBackTransaction();

            var riskRefreshed = repository.GetFirst(new {RiskId = risk.RiskId});

            Assert.IsNull(riskRefreshed);



        }


        [TestMethod]
        public void Should_page_a_query_using_the_default_identity_sort()
        {
            var unitOfWork = new UnitOfWork();

            //Given
            var risk = new Risk();
            risk.Address1 = "Crescent";
            risk.Address2 = "Tyne & Wear";
            risk.Name = "Home";
            risk.PostCode = "NE30";
            risk.PolicyId = 2;


            var repository = CreateNewRiskRepository();
            
            unitOfWork.BeginTransaction();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    var inserted = repository.Insert(risk);

                    Assert.IsTrue(inserted);
                    Assert.IsTrue(risk.RiskId > 0);
                }

                //When
                var riskRefreshed = repository.GetWhere(new {PolicyId = risk.PolicyId}, 1, 5);
                //Then
                Assert.AreEqual(5, riskRefreshed.Count());
            }
            finally
            {
                //Teardown
                unitOfWork.RollBackTransaction();
            }
        }



        [TestMethod]
        public void Should_count_using_filter()
        {
            var unitOfWork = new UnitOfWork();

            //Given
            var risk = new Risk();
            risk.Address1 = "Crescent";
            risk.Address2 = "Tyne & Wear";
            risk.Name = "Home";
            risk.PostCode = "NE30";
            risk.PolicyId = 2;


            var repository = CreateNewRiskRepository();

            unitOfWork.BeginTransaction();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    var inserted = repository.Insert(risk);

                    Assert.IsTrue(inserted);
                    Assert.IsTrue(risk.RiskId > 0);
                }

                //When
                var riskCount = repository.GetCount(new { PolicyId = risk.PolicyId });
                //Then
                Assert.AreEqual(10, riskCount);
            }
            finally
            {
                //Teardown
                unitOfWork.RollBackTransaction();
            }
        }
    }
}
