using System;
using Xunit;
using Catalog.Controllers;
using Moq; 

namespace Catalog_Test
{
    public class UnitTest1
    {
        
        [Fact]
        public void Test1()
        {
             

           // var itemCont = new Mock();
            var ProItemCont = new Catalog.Controllers.ItemsController();

            Assert.NotNull(ProItemCont.GetAll());

            //int a = 1;
            //int b = 2;

            //Assert.Equal(3, a + b);



        }


        [Fact]
        public void Test2()
        {
            //Catalog.Controllers.ItemsController CalCon = new ItemsController() ;

            int a = 1;
            int b = 2;              

            Assert.Equal(2, a + b);
            Assert.Equal(3, a + b);
            //Assert.NotNull(a);

        }

    }
}
