﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.Tests {

	[TestClass]
	public class UnitTest1 {

		[TestMethod]
		public void Can_Paginate() {
			ProductsListViewModel model = getListViewModel(page: 2);
			Product[] prodArray = model.Products.ToArray();

			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].Name, "P4");
			Assert.AreEqual(prodArray[1].Name, "P5");
		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model() {
			ProductsListViewModel model = getListViewModel(page: 2);
			PagingInfo pageInfo = model.PagingInfo;

			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Filter_Products() {
			Product[] result = getListViewModel("Cat2", 1).Products.ToArray();

			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
			Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
		}

		[TestMethod]
		public void Can_Remove_Duplicate_Categories() {
			Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Apples"},
				new Product {ProductID = 2, Name = "P2", Category = "Apples"},
				new Product {ProductID = 3, Name = "P3", Category = "Plums"},
				new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
			});

			NavController target = new NavController(mock.Object);
			string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

			Assert.AreEqual(results.Length, 3);
			Assert.AreEqual("Apples", results[0]);
			Assert.AreEqual("Oranges", results[1]);
			Assert.AreEqual("Plums", results[2]);
		}

		[TestMethod]
		public void Can_Highlight_Selected_Category() {
			Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductID = 1, Name = "P1", Category = "Apples"},
				new Product {ProductID = 2, Name = "P2", Category = "Oranges"},
			});

			NavController controller = new NavController(mock.Object);
			string categoryToSelect = "Apples";
			string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

			Assert.AreEqual(categoryToSelect, result);
		}

		[TestMethod]
		public void Can_Create_Category_Specific_Product_Count() {
			int category1 = getListViewModel("Cat1").PagingInfo.TotalItems;
			int category2 = getListViewModel("Cat2").PagingInfo.TotalItems;
			int category3 = getListViewModel("Cat3").PagingInfo.TotalItems;
			int allCategories = getListViewModel().PagingInfo.TotalItems;

			Assert.AreEqual(2, category1);
			Assert.AreEqual(2, category2);
			Assert.AreEqual(1, category3);
			Assert.AreEqual(5, allCategories);
		}

		private static ProductsListViewModel getListViewModel(string category = null, int page = 1) {
			Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product { ProductID = 1, Name = "P1", Category = "Cat1"},
				new Product { ProductID = 2, Name = "P2", Category = "Cat2"},
				new Product { ProductID = 3, Name = "P3", Category = "Cat1"},
				new Product { ProductID = 4, Name = "P4", Category = "Cat2"},
				new Product { ProductID = 5, Name = "P5", Category = "Cat3"}
			});

			ProductController controller = new ProductController(mock.Object);
			controller.pageSize = 3;
			ProductsListViewModel model = (ProductsListViewModel)controller.List(category, page).Model;

			return model;
		}

		[TestMethod]
		public void Can_Generate_Page_Links() {
			System.Web.Mvc.HtmlHelper myHelper = null;
			PagingInfo pagingInfo = new PagingInfo {
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};

			Func<int, string> PageUrlDelegate = i => "Page" + i;

			MvcHtmlString result = myHelper.PageLinks(pagingInfo, PageUrlDelegate);

			string expected = @"<a class=""btn btn-default"" href=""Page1"">1</a>"
			+ @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
			+ @"<a class=""btn btn-default"" href=""Page3"">3</a>";
			Assert.AreEqual(expected, result.ToString());
		}
	}
}