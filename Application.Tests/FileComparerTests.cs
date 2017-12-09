using Applications;
using FluentAssertions;
using Interfaces.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Application.Tests
{
    [TestClass]
    public class FileComparerTests
    {
        FileComparer _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new FileComparer();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void WHEN_comparing_indentical_files_THEN_retunrs_equal()
        {
            // assign
            var file = new TextFile()
            {
                Id = "id",
                Data = "data"
            };

            var file2 = new TextFile()
            {
                Id = "id2",
                Data = "data"
            };

            // act
            var result = _target.Diff(file, file2);

            // assert
            result.Status.Should().Be(DiffStatus.Equal);
            result.Summary.Should().Be("The files are equal");
        }

        [TestMethod]
        public void GIVEN__the_first_file_is_larger_than_the_second_WHEN_comparing_files_THEN_information_about_the_size()
        {
            // assign
            var file = new TextFile()
            {
                Id = "id",
                Data = "data data"
            };

            var file2 = new TextFile()
            {
                Id = "id2",
                Data = "data"
            };

            // act
            var result = _target.Diff(file, file2);

            // assert
            result.Status.Should().Be(DiffStatus.NotEqual);
            result.Summary.Should().Be("The right file is 5 bytes larger than the left one.");
        }

        [TestMethod]
        public void GIVEN_lenght_of_the_files_are_different_WHEN_comparing_files_THEN_information_about_the_size()
        {
            // assign
            var file = new TextFile()
            {
                Id = "id",
                Data = "data"
            };

            var file2 = new TextFile()
            {
                Id = "id2",
                Data = "data data data"
            };

            // act
            var result = _target.Diff(file, file2);

            // assert
            result.Status.Should().Be(DiffStatus.NotEqual);
            result.Summary.Should().Be("The right file is 10 bytes smaller than the left one.");
        }
    }
}
