using System;
using System.Linq;
using NUnit.Framework;
using PCRemote.DataAccess;
using SubSonic.Repository;

namespace PCRemote.Tests
{
    [TestFixture]
    public class DataAccessTests
    {
        PCRemoteDB _db;
        SimpleRepository _repo;

        [SetUp]
        public void Init()
        {
            _db = new PCRemoteDB();
            _repo = new SimpleRepository(_db.Provider);
        }

        [Test]
        public void Insert_Data_Test()
        {

            // Arrange
            var user = new Command()
            {
                CommandName = "Test"
            };

            // Act
            _repo.Add(user);

            // Assert

        }

        [Test]
        public void Update_Data_Test()
        {
            // Arrange
            var user = _repo.Find<Command>(u => u.CommandName == "Test").SingleOrDefault();

            // Act
            user.CommandName = "Update User Name";
            _repo.Update(user);

            // Assert

        }

        [Test]
        public void Delete_Data_Test()
        {
            // Arrange

            // Act
            _repo.Delete<Command>("Test");

            // Assert

        }

        [Test]
        public void GetData_Test()
        {
            var db = new PCRemoteDB();

            var users = (from u in db.Commands
                         select u).ToList();

            Assert.IsTrue(users.Count > 0);
        }
    }
}