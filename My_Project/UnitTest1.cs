using NUnit.Framework;
using 

namespace My_Project
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.google.com/");
            driver.Close();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}