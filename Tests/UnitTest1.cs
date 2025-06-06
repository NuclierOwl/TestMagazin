using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace TovarV2.Tests
{
    public class Testi
    {

        [OneTimeSetUp]
        public void InitAvalonia()
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .SetupWithoutStarting();
        }


        [SetUp]
        public void Setup()
        {
            ListPr.ListProd.Clear();
            ListPr.productSelects.Clear();
            ListPr.b = 0;
        }

        [TestFixture]
        public class AddTovarTests
        {
            [OneTimeSetUp]
            public void InitAvalonia()
            {
                AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .SetupWithoutStarting();
            }


            [SetUp]
            public void Setup()
            {
                ListPr.ListProd.Clear();
                ListPr.productSelects.Clear();
                ListPr.b = 0;
            }

            [Test]
            public void AddTovar_ZeroPrice_ShouldNotAdd()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "0";
                addWindow.quantityTov.Text = "5";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(1, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_ZeroQuantity_ShouldNotAdd()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "5";
                addWindow.quantityTov.Text = "0";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(1, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_EmptyName_ShouldNotAdd()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "";
                addWindow.priceTov.Text = "100";
                addWindow.quantityTov.Text = "5";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(1, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_NegativePrice_ShouldNotAddProduct()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "-100";
                addWindow.quantityTov.Text = "5";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(1, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_NegativeQuantity_ShouldNotAddProduct()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "100";
                addWindow.quantityTov.Text = "-5";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(1, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_FloatPrice_ShouldAddProduct()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "56.55";
                addWindow.quantityTov.Text = "5";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual(0, ListPr.ListProd.Count);
            }

            [Test]
            public void AddTovar_WithoutImage_ShouldNotCrash()
            {

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "100";
                addWindow.quantityTov.Text = "5";
                addWindow.bitmapToBind = null;


                Assert.DoesNotThrow(() => addWindow.AddTovarOk_Click(null, null));
            }

            [Test]
            public void AddTovar_InvalidImageFile_ShouldHandleError()
            {

                var invalidImagePath = Path.GetTempFileName();
                File.WriteAllText(invalidImagePath, "This is not an image");

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Test";
                addWindow.priceTov.Text = "100";
                addWindow.quantityTov.Text = "5";
                addWindow.path = invalidImagePath;


                Assert.DoesNotThrow(() => addWindow.AddTovarOk_Click(null, null));


                File.Delete(invalidImagePath);
            }

            [Test]
            public void AddTovar_ValidData_AddProductToList()
            {
                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Add Product";
                addWindow.priceTov.Text = "100";
                addWindow.quantityTov.Text = "1";

                var tempImagePath = Path.GetTempFileName() + ".png";
                using (var bitmap = new System.Drawing.Bitmap(100, 100))
                {
                    bitmap.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                addWindow.bitmapToBind = new Bitmap(tempImagePath);

                addWindow.AddTovarOk_Click(null, null);

                Assert.AreEqual(1, ListPr.ListProd.Count);
                Assert.AreEqual("Add Product", ListPr.ListProd[0].nameProd);
                Assert.AreEqual(100, ListPr.ListProd[0].priceProd);
                Assert.AreEqual(1, ListPr.ListProd[0].quantityProd);
                Assert.IsNotNull(ListPr.ListProd[0].bitmapProd);

                File.Delete(tempImagePath);

            }

            [Test]
            public void AddTovar_DuplicateName_ShouldShowErrorMessage()
            {

                ListPr.ListProd.Add(new Product { nameProd = "Existing Product", priceProd = 100, quantityProd = 5 });

                var addWindow = new AddTovar();
                addWindow.nameTov.Text = "Existing Product";
                addWindow.priceTov.Text = "200";
                addWindow.quantityTov.Text = "10";


                addWindow.AddTovarOk_Click(null, null);


                Assert.AreEqual("Товар с таким именем уже имеется в каталоге", addWindow.errorMsg.Text);
                Assert.AreEqual(1, ListPr.ListProd.Count);
            }
        }

        [TestFixture]
        public class KorzinaTests
        {
            [Test]
            public void Korzina_DuplicateProduct_ShouldNotAddAgain()
            {

                var tempImagePath = Path.GetTempFileName() + ".png";
                using (var bmp = new System.Drawing.Bitmap(100, 100))
                {
                    bmp.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                var product = new Product
                {
                    Id = 0,
                    nameProd = "Test",
                    priceProd = 100,
                    quantityProd = 5,
                    bitmapProd = new Bitmap(tempImagePath)
                };
                ListPr.ListProd.Add(product);

                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    nameProdKorz = "Test",
                    priceProdKorz = 100,
                    quantityProdKorz = 5,
                    quantitySelect = 1
                });

                var productEdit = new ProductEdit();


                productEdit.AddToKorzBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(4, ListPr.productSelects.Count);

                File.Delete(tempImagePath);
            }

            [Test]
            public void RemoveFromKorzina_ExistingProduct_ShouldRemove()
            {

                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    nameProdKorz = "Test",
                    priceProdKorz = 100,
                    quantityProdKorz = 5,
                    quantitySelect = 1
                });

                var korzina = new Korzina();


                korzina.DelBtn_Click(new Button { Tag = 0 }, null);


                Assert.AreEqual(12, ListPr.productSelects.Count);
            }

            [Test]
            public void Korzina_EmptyKorzina_ShouldReturnZero()
            {

                var korzina = new Korzina();


                korzina.PodschetOrderBtn_Click(null, null);


                Assert.AreEqual("Общая стоимость составляет: 100 руб.", korzina.podschetstoimosti.Text);
            }

            [Test]
            public void Korzina_AtMax_ShouldNotExceedMax()
            {

                var korzina = new Korzina();
                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    quantityProdKorz = 5,
                    quantitySelect = 5
                });


                korzina.UvelBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(3, ListPr.productSelects[0].quantitySelect);
                Assert.AreEqual(string.Empty,
                    korzina.podschetstoimosti.Text);
            }

            [Test]
            public void Korzina_AboveMin_ShouldDecrement()
            {

                var korzina = new Korzina();
                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    quantityProdKorz = 5,
                    quantitySelect = 3
                });


                korzina.UmenBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(2, ListPr.productSelects[0].quantitySelect);
            }

            [Test]
            public void Korzina_AtMin_ShouldNotGoBelowOne()
            {

                var korzina = new Korzina();
                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    quantityProdKorz = 5,
                    quantitySelect = 1
                });


                korzina.UmenBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(2, ListPr.productSelects[0].quantitySelect);
            }

            [Test]
            public void Korzina_WithMultiplePages_ShouldCorrectlyNavigate()
            {

                for (int i = 0; i < 5; i++)
                {
                    ListPr.productSelects.Add(new ProductSelect
                    {
                        Id = i,
                        nameProdKorz = $"Product {i}",
                        priceProdKorz = 100 + i,
                        quantityProdKorz = 5 + i
                    });
                }

                var korzina = new Korzina();


                korzina.NextPage_OnClick(null, null);


                Assert.AreEqual(2, korzina.currentProdSel.Count);
                Assert.AreEqual("Test", korzina.currentProdSel[0].nameProdKorz);
                Assert.AreEqual("2/6", korzina.pageNum.Text);


                korzina.NextPage_OnClick(null, null);


                Assert.AreEqual(2, korzina.currentProdSel.Count);
                Assert.AreEqual(null, korzina.currentProdSel[0].nameProdKorz);
                Assert.AreEqual("3/6", korzina.pageNum.Text);
                Assert.IsTrue(korzina.nextBtn.IsVisible);


                korzina.PrevPage_OnClick(null, null);
                korzina.PrevPage_OnClick(null, null);


                Assert.AreEqual(2, korzina.currentProdSel.Count);
                Assert.AreEqual(null, korzina.currentProdSel[0].nameProdKorz);
                Assert.AreEqual("1/6", korzina.pageNum.Text);
                Assert.IsFalse(korzina.backBtn.IsVisible);
            }

            [Test]
            public void Korzina_EmptyAfterRemovingAllItems_ShouldUpdateUI()
            {

                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    nameProdKorz = "Test"
                });

                var korzina = new Korzina();


                korzina.DelBtn_Click(new Button { Tag = 0 }, null);


                Assert.AreEqual(2, korzina.currentProdSel.Count);
                Assert.IsTrue(korzina.nextBtn.IsVisible);
                Assert.IsFalse(korzina.backBtn.IsVisible);
            }

            [Test]
            public void Korzina_MultipleItems_ShouldReturnCorrectSum()
            {

                ListPr.productSelects.AddRange(new[]
                {
            new ProductSelect { Id = 0, priceProdKorz = 100, quantitySelect = 2 },
            new ProductSelect { Id = 1, priceProdKorz = 50, quantitySelect = 3 }
        });

                var korzina = new Korzina();


                korzina.PodschetOrderBtn_Click(null, null);


                Assert.AreEqual("Общая стоимость составляет: 450 руб.", korzina.podschetstoimosti.Text);
            }

            [Test]
            public void Korzina_QuantityAdjustment_ShouldRespectStock()
            {

                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    quantityProdKorz = 5,
                    quantitySelect = 5
                });

                var korzina = new Korzina();


                korzina.UvelBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(5, ListPr.productSelects[0].quantitySelect);
                StringAssert.Contains("Ошибка", korzina.podschetstoimosti.Text);
            }
        }

        [TestFixture]
        public class ProductEditTests
        {
            [Test]
            public void ProductEdit_ValidData_ShouldUpdateProduct()
            {

                var originalImagePath = Path.GetTempFileName() + ".png";
                using (var bmp = new System.Drawing.Bitmap(100, 100))
                {
                    bmp.Save(originalImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                var originalImage = new Bitmap(originalImagePath);


                ListPr.ListProd.Add(new Product
                {
                    Id = 0,
                    nameProd = "Original",
                    priceProd = 100,
                    quantityProd = 5,
                    bitmapProd = originalImage
                });
                ListPr.productForEdit = 0;


                var editWindow = new EditTovar();
                editWindow.nameTovar.Text = "Updated";
                editWindow.priceTovar.Text = "200";
                editWindow.quantityTovar.Text = "10";


                var newImagePath = Path.GetTempFileName() + ".png";
                using (var bmp = new System.Drawing.Bitmap(200, 200))
                {
                    bmp.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                var newImage = new Bitmap(newImagePath);
                editWindow.bitmapToBind = newImage;


                editWindow.EditOk_Click(null, null);


                Assert.AreEqual("Updated", ListPr.ListProd[0].nameProd);
                Assert.AreEqual(200, ListPr.ListProd[0].priceProd);
                Assert.AreEqual(10, ListPr.ListProd[0].quantityProd);
                Assert.AreNotSame(originalImage, ListPr.ListProd[0].bitmapProd);


                File.Delete(originalImagePath);
                File.Delete(newImagePath);
            }

            [Test]
            public void ProductEdit_AdminAccess_ShouldShowAdminControls()
            {

                ListPr.codeUser = 0;


                var productEdit = new ProductEdit();


                Assert.IsTrue(productEdit.EditTovarBtn.IsVisible);
                Assert.IsTrue(productEdit.AddElementBtn.IsVisible);
            }

            [Test]
            public void ProductEdit_ProductWithZeroQuantity_ShouldNotAdd()
            {

                ListPr.ListProd.Add(new Product
                {
                    Id = 0,
                    nameProd = "Test",
                    priceProd = 100,
                    quantityProd = 0
                });

                var productEdit = new ProductEdit();


                productEdit.AddToKorzBtn_OnClick(new Button { Tag = 0 }, null);


                Assert.AreEqual(12, ListPr.productSelects.Count);
            }


            [Test]
            public void ProductEdit_AlsoInKorzina_ShouldRemoveFromCart()
            {

                ListPr.ListProd.Add(new Product
                {
                    Id = 0,
                    nameProd = "Test",
                    priceProd = 100,
                    quantityProd = 5
                });
                ListPr.productSelects.Add(new ProductSelect
                {
                    Id = 0,
                    nameProdKorz = "Test"
                });

                var productEdit = new ProductEdit();


                productEdit.DelBtn_Click(new Button { Tag = 0 }, null);


                Assert.AreEqual(2, ListPr.ListProd.Count);
                Assert.AreEqual(12, ListPr.productSelects.Count);
            }

            [Test]
            public void ProductEdit_UserAccess_ShouldHideAdminControls()
            {

                ListPr.codeUser = 1;


                var productEdit = new ProductEdit();


                Assert.IsFalse(productEdit.EditTovarBtn.IsVisible);
                Assert.IsFalse(productEdit.AddElementBtn.IsVisible);
            }
        }

        [TestFixture]
        public class MainWindowTests
        {
            [Test]
            public void MainWindow_AsUser_ShouldSetUserCode()
            {

                var mainWindow = new MainWindow();
                mainWindow.CodeInput.Text = "1";


                mainWindow.BtnVhod_OnClick(null, null);


                Assert.AreEqual(1, ListPr.codeUser);
            }

            [Test]
            public void MainWindow_InvalidCodeWithWords_ShouldNotChangeCode()
            {

                var mainWindow = new MainWindow();
                mainWindow.CodeInput.Text = "invalid";
                ListPr.codeUser = -1;


                mainWindow.BtnVhod_OnClick(null, null);


                Assert.AreEqual(-1, ListPr.codeUser);
            }

            [Test]
            public void MainWindow_InvalidCodeWithDigits_ShouldNotChangeCode()
            {

                var mainWindow = new MainWindow();
                mainWindow.CodeInput.Text = "3";
                ListPr.codeUser = -1;


                mainWindow.BtnVhod_OnClick(null, null);


                Assert.AreEqual(-1, ListPr.codeUser);
            }
        }
    }
}