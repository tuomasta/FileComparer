using Application.Repository;
using Applications.Repository;
using FluentAssertions;
using Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Application.Tests
{
    [TestClass]
    public class TextRepositoryTests
    {
        Context _testContext;
        TextRepository _target;

        [TestInitialize]
        public void Initialize()
        {
            // Testing using concrete db context with in memory configuration.
            // This provides more realistic test results than mocking the db context.
            var options = new DbContextOptionsBuilder<Context>()
             .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
             .Options;

            _testContext = new Context(options);
            _target = new TextRepository(_testContext);
            _testContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // this is critical to ensure that the tests does not effect to other ones
            _testContext.Database.EnsureDeleted();
            _testContext.Dispose();
        }

        [TestMethod]
        public async Task GIVEN_database_is_empty_WHEN_getting_files_THEN_returns_empty_collection()
        {
            // act
            var result = await _target.GetFilesAsync();

            // assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task GIVEN_database_is_empty_WHEN_getting_a_file_THEN_returns_null()
        {
            // act
            var result = await _target.GetFileAsync("non existing id");

            // assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GIVEN_database_is_empty_WHEN_saving_a_file_AND_fetching_the_saved_THEN_returns_the_saved_file()
        {
            // assign
            var file = new TextFile()
            {
                Data = "data",
                Id = Guid.NewGuid().ToString()
            };

            // act
            await _target.SaveFileAsync(file);
            var result = await _target.GetFileAsync(file.Id);

            // assert
            result.Id.Should().Be(file.Id);
            result.Data.Should().Be(file.Data);
        }

        [TestMethod]
        public async Task GIVEN_a_file_exists_in_db_WHEN_deleting_the_file_THEN_the_file_gets_deleted()
        {
            // assign
            var file = new TextFile()
            {
                Data = "data",
                Id = Guid.NewGuid().ToString()
            };
            _testContext.Files.Add(file);
            await _testContext.SaveChangesAsync();

            // act
            await _target.DeleteFileAsync(file.Id);

            // assert
            var result = _testContext.Files.Find(file.Id);
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GIVEN_a_file_exists_in_db_WHEN_saving_another_with_same_id_THEN_throws_exception()
        {
            // assign
            var file = new TextFile()
            {
                Data = "data",
                Id = Guid.NewGuid().ToString()
            };

            _testContext.Files.Add(file);
            await _testContext.SaveChangesAsync();

            // act
            Func<Task> act = async () => await _target.SaveFileAsync(file);

            // assert
            act.ShouldThrow<Exception>();
        }
    }
}
