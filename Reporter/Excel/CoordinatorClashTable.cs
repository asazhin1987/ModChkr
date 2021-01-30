using Coordinator.DTO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporter.Excel
{
	public static partial class ExcelFactory
	{
		public static void Create(string path, IEnumerable<ClashDTO> items)
		{
			using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
			{
				//=============создание документа
				WorkbookPart workbookPart = document.AddWorkbookPart();
				workbookPart.Workbook = new Workbook();
				Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

				WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();

				stylePart.Stylesheet = GenerateStylesheet();
				stylePart.Stylesheet.Save();

				uint startrow = 1;
				WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

				Worksheet workSheet = new Worksheet();
				SheetData sheetData = new SheetData();
				MergeCells mergeCells = new MergeCells();
				//=============шапка документа
				startrow = CreateHeader(sheetData, startrow);
				//=============тело таблицы
				foreach (var check in items.AsParallel().AsOrdered())
				{
					startrow = CreateCheckRow(check, sheetData, startrow);
				}

				////=============ширина столбов
				SetColumnWidth(workSheet, new List<double>
				{6, 15, 20, 20, 20, 40, 40, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20});
				workSheet.AppendChild(sheetData);


				worksheetPart.Worksheet = workSheet;
				worksheetPart.Worksheet.InsertAt(GetSheetViews(true, 3D, "A4"), 0);
				Sheet sheet = CreateSheets(workbookPart, worksheetPart, "Report", 1);
				sheets.Append(sheet);
				ApplyAutofilter(worksheetPart, "B3:U3");

				//=============сохраняем книгу
				workbookPart.Workbook.Save();
				document.Close();
			}
		}

		private static uint CreateHeader(SheetData sheetData, uint startrow)
		{
			//bimacad
			Row row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 40 };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);
			InsertCell(row, 2, "BIMACAD MODEL COORDINATOR", CellValues.String, FCellFormats.GreenBold12);
			sheetData.Append(row);
			startrow++;
			//date
			row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 30 };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);
			InsertCell(row, 2, $"Отчет по коллизиям - {DateTime.Now.ToString()}", CellValues.String, FCellFormats.DarkGreenBold1);
			sheetData.Append(row);
			startrow++;

			//шапка таблицы
			int c = 0;
			row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 50 };
			InsertCell(row, ++c, "", CellValues.String, FCellFormats.Default);

			InsertCell(row, ++c, "ID", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Объект", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Проверка", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Статус", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Модель 1", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Модель 2", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "ID Элемент 1", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "ID Элемент 2", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Элемент 1", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Элемент 2", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Уровень 1", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Уровень 2", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Категория 1", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Категория 2", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Расстояние", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Дата обнаружения", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Дата изменения", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "X", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Y", CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, ++c, "Z", CellValues.String, FCellFormats.BorderCenterBold);

			sheetData.Append(row);
			startrow++;
			return startrow;
		}


		private static uint CreateCheckRow(ClashDTO item, SheetData sheetData, uint startrow)
		{
			int c = 0;
			Row row = new Row() { RowIndex = startrow };
			InsertCell(row, ++c, "", CellValues.String, FCellFormats.Default);

			InsertCell(row, ++c, item.Id.ToString(), CellValues.Number, FCellFormats.BorderInt);
			InsertCell(row, ++c, item.ConstructionName, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.CheckName, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.ClashStatusName, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.RevitModel1Name, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.RevitModel2Name, CellValues.String, FCellFormats.BorderLeftNormal);

			InsertCell(row, ++c, item.RevitElement1Id.ToString(), CellValues.Number, FCellFormats.BorderInt);
			InsertCell(row, ++c, item.RevitElement2Id.ToString(), CellValues.Number, FCellFormats.BorderInt);

			InsertCell(row, ++c, item.RevitElement1Name, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.RevitElement2Name, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.Element1Level, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.Element2Level, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.CategoryElement1Name, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, ++c, item.CategoryElement2Name, CellValues.String, FCellFormats.BorderLeftNormal);

			InsertCell(row, ++c, ReplaceMoneySymbols(item.Distance.ToString()), CellValues.Number, FCellFormats.BorderDouble);
			InsertCell(row, ++c, item.FoundDate, CellValues.String, FCellFormats.BorderDate);
			InsertCell(row, ++c, item.Odate, CellValues.String, FCellFormats.BorderDate);

			InsertCell(row, ++c, ReplaceMoneySymbols(item.X.ToString()), CellValues.Number, FCellFormats.BorderDouble);
			InsertCell(row, ++c, ReplaceMoneySymbols(item.Y.ToString()), CellValues.Number, FCellFormats.BorderDouble);
			InsertCell(row, ++c, ReplaceMoneySymbols(item.Z.ToString()), CellValues.Number, FCellFormats.BorderDouble);

			sheetData.Append(row);

			return ++startrow;
		}

	}
}
