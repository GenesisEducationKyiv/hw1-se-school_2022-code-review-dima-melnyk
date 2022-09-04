using FluentAssertions;
using GSES.DataAccess.Consts;
using GSES.DataAccess.Entities.Bases;
using GSES.DataAccess.Storages.File;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace GSES.DataAccess.Tests
{
    public class FileTests
    {
        [Fact]
        public void AddAsync_ThrowsDuplicateException()
        {
            // Arrange
            var fileMock = new Mock<File<TestEntity>>(new ConfigurationBuilder().Build());
            fileMock.Setup(f => f.GetAllAsync()).ReturnsAsync(this.GetTestData());
            var duplicateEntity = new TestEntity() { String = "String2" };

            // Act + Assert
            Assert.ThrowsAsync<DuplicateNameException>(() => fileMock.Object.AddAsync(duplicateEntity));
        }

        [Fact]
        public async Task AddAsync_AddsElementSuccessfully()
        {
            // Arrange
            var configMock = this.GetTestConfiguration();
            var file = new File<TestEntity>(configMock.Object);
            var entity = new TestEntity() { String = "String1" };
            var filePath = configMock.Object[FileConsts.FilePathConfig];

            // Act
            await file.AddAsync(entity);

            // Assert
            var serializedElements = await File.ReadAllTextAsync(filePath + typeof(TestEntity) + GeneralConsts.JsonExtension);
            var elements = JsonConvert.DeserializeObject<IEnumerable<TestEntity>>(serializedElements);
            Assert.Contains(entity, elements);
        }

        [Fact]
        public async Task GetAllAsync_FileDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var configMock = this.GetTestConfiguration();
            var file = new File<TestEntity>(configMock.Object);

            // Act
            var list = await file.GetAllAsync();

            // Assert
            list.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListSuccessfully()
        {
            // Arrange
            var configMock = this.GetTestConfiguration();
            var file = new File<TestEntity>(configMock.Object);
            var jsonModel = JsonConvert.SerializeObject(GetTestData());
            var filePath = configMock.Object[FileConsts.FilePathConfig];
            File<TestEntity>.EnsureFolderExists(filePath);

            using (var fileStream = new FileStream(filePath + typeof(TestEntity) + GeneralConsts.JsonExtension, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using var streamWriter = new StreamWriter(fileStream);
                await streamWriter.WriteAsync(jsonModel);
            }

            // Act
            var list = await file.GetAllAsync();

            // Assert
            list.Should().BeEquivalentTo(GetTestData());
        }

        private IEnumerable<TestEntity> GetTestData() => new List<TestEntity>
        {
            new TestEntity() { String = "String1" },
            new TestEntity() { String = "String2" },
            new TestEntity() { String = "String3" },
        };

        private Mock<IConfiguration> GetTestConfiguration()
        {
            var mock = new Mock<IConfiguration>();
            mock.Setup(c => c[FileConsts.FilePathConfig]).Returns(GenerateTestFolderPath());

            return mock;
        }

        private string GenerateTestFolderPath() => "..\\" + Guid.NewGuid().ToString() + "\\";
    }

    public class TestEntity : BaseEntity
    {
        public string String { get; set; }

        public override bool Equals(object obj)
        {
            return ((TestEntity)obj).String.Equals(this.String);
        }
    }
}
