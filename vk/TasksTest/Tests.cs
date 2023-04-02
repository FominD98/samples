using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Tasks;

namespace TasksTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestGenerateLogins()
        {
            var testFilePath = "Files/1/TestData.txt";
            var expectedOutputFilePath = "Files/1/ExpectedOutput.txt";
        
            new LoginGenerator(testFilePath).Generate("output.txt");
            
            var actualLogins = File.ReadAllLines("output.txt").ToList();
            var expectedLogins = File.ReadAllLines(expectedOutputFilePath).ToList();
            actualLogins.Should().BeEquivalentTo(expectedLogins);
        }
        [Test]
        public void TestExchangeData()
        {
            var startDataFile = "Files/2/StartData.json";
            var employeesFile = "Files/2/Employees.json";
            var expectedFile = "Files/2/FinalData.json";
            var actualFile = "ActualFinalData.json";
        
            new DataExchanger(startDataFile, employeesFile).Exchange(actualFile);
        
            var expected = JsonConvert.DeserializeObject<List<IDMData>>(File.ReadAllText(expectedFile));
            var actual = JsonConvert.DeserializeObject<List<IDMData>>(File.ReadAllText(actualFile));
            expected.Should().BeEquivalentTo(actual);
        }
    }
}