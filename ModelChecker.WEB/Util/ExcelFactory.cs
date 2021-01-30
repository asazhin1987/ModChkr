using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ModelChecker.DTO;
using ModelChecker.WEB.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace ModelChecker.WEB.Util
{
	public class ExcelFactory
	{
		public static void CreateExcelReporChecks(ChecksPageViewModel model, string path)
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
				startrow = CreateHeader(sheetData, startrow, model.Construction.Name);
				//=============тело таблицы
				foreach (var check in model.Checks.AsParallel().AsOrdered())
				{
					startrow = CreateCheckRow(check, sheetData, startrow);
				}
				////=============footer
				//startrow = CreateFooterRow(sheetData, startrow, mergeCells, orders.Sum(x => x.Labor));
				////=============ширина столбов
				SetColumnWidth(workSheet, new Dictionary<int, double>()
						{ {1, 2 }, { 2, 40 }, { 3, 20 }, { 4, 20 }, { 5, 20 }, { 6, 20 }, { 7, 20 }, { 8, 20 }, { 9, 20 } });
				workSheet.AppendChild(sheetData);


				worksheetPart.Worksheet = workSheet;
				worksheetPart.Worksheet.InsertAt(GetSheetViews(true), 0);
				Sheet sheet = CreateSheets(workbookPart, worksheetPart, "Report", 1);
				sheets.Append(sheet);
				ApplyAutofilter(worksheetPart, "B3:I3");
				
				//workSheet.AppendChild(mergeCells);

				//=============сохраняем книгу
				workbookPart.Workbook.Save();
				document.Close();
			}
		}

		//private static uint CreateFooterRow(SheetData sheetData, uint startrow, MergeCells merge, int sum)
		//{
		//	TimeSpan ts = TimeSpan.FromMinutes(sum);
		//	string s = $"{ts.Hours + ts.Days * 24 }:{ts.Minutes}";
		//	Row row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 30 };
		//	InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);

		//	InsertCell(row, 2, "Итого:", CellValues.String, FCellFormats.BorderRightBold);
		//	InsertCell(row, 3, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 4, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 5, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 6, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 7, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 8, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 9, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 10, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 11, "", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 12, $"{s}", CellValues.String, FCellFormats.BorderCenterBold);
		//	InsertCell(row, 13, "", CellValues.String, FCellFormats.BorderCenterBold);

		//	InsertCell(row, 14, "", CellValues.String, FCellFormats.Default);
		//	sheetData.Append(row);
		//	MergeCells(merge, "B", "K", startrow);

		//	return ++startrow;
		//}

	

		private static uint CreateCheckRow(FullCheckDTO item, SheetData sheetData, uint startrow)
		{
			Row row = new Row() { RowIndex = startrow };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);

			InsertCell(row, 2, item.Name, CellValues.String, FCellFormats.BorderLeftNormal);
			InsertCell(row, 3, item.AtWorkClashesQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 4, item.CreatedQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 5, item.ActiveQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 6, item.AnalizedQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 7, item.ConfirmedQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 8, item.CorrectedQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			//InsertCell(row, 9, item.AtWorkClashesQnt.ToString(), CellValues.Number, FCellFormats.BorderNumber);
			InsertCell(row, 9, item.Date, CellValues.String, FCellFormats.BorderDate);

			sheetData.Append(row);

			return ++startrow;
		}

		private static uint CreateHeader(SheetData sheetData, uint startrow, string constrName)
		{
			//bimacad
			Row row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 40 };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);
			InsertCell(row, 2, "BIMACAD MODEL CHECKER", CellValues.String, FCellFormats.GreenBold12);
			sheetData.Append(row);
			startrow++;
			//date
			row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 30 };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);
			InsertCell(row, 2, $"{constrName} - {DateTime.Now.ToString()}", CellValues.String, FCellFormats.DarkGreenBold1);
			sheetData.Append(row);
			startrow++;
			
			//шапка таблицы
			row = new Row() { RowIndex = startrow, CustomHeight = true, Height = 50 };
			InsertCell(row, 1, "", CellValues.String, FCellFormats.Default);

			InsertCell(row, 2, Resources.Global.CheckName, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 3, Resources.Global.Clashes, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 4, Resources.Global.StatusCreate, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 5, Resources.Global.StatusActive, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 6, Resources.Global.StatusAnalized, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 7, Resources.Global.StatusConfirmed, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 8, Resources.Global.StatusCorrected, CellValues.String, FCellFormats.BorderCenterBold);
			InsertCell(row, 9, Resources.Global.Date, CellValues.String, FCellFormats.BorderCenterBold);
			sheetData.Append(row);
			startrow++;
			return startrow;
		}


		private static void InsertCell(Row row, int cell_num, string val, CellValues type, FCellFormats style)
		{
			InsertCell(row, cell_num, val, type, (uint)style);
		}
		private static void InsertCell(Row row, int cell_num, DateTime? val, CellValues type, FCellFormats style)
		{
			string strdate = val?.ToString(CultureInfo.InvariantCulture);
			InsertCell(row, cell_num, strdate, type, style);
		}

		private static void InsertCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
		{
			Cell refCell = null;
			Cell newCell = new Cell() { CellReference = $"{cell_num}:{row.RowIndex}", StyleIndex = styleIndex };
			row.InsertBefore(newCell, refCell);
			newCell.CellValue = new CellValue(val);
			newCell.DataType = new EnumValue<CellValues>(type);
		}

		

		private static void ApplyAutofilter(WorksheetPart worksheetPart, string reference)
		{
			var autoFilter = new AutoFilter() { Reference = reference };
			OpenXmlElement preceedingElement = GetPreceedingElement(worksheetPart);
			worksheetPart.Worksheet.InsertAfter(autoFilter, preceedingElement);

		}

		//private static void MergeCells(MergeCells merge, string literA, string literB, uint row)
		//{
		//	merge.Append(new MergeCell() { Reference = new StringValue($"{literA}{row}:{literB}{row}") });
		//}

		//private static void MergeCells(MergeCells merge, string CellA, string CellB)
		//{
		//	merge.Append(new MergeCell() { Reference = new StringValue($"{CellA}:{CellB}") });
		//}

		private static Sheet CreateSheets(WorkbookPart workbookPart, WorksheetPart worksheetPart, string name, UInt32Value ShId)
		{
			return new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = ShId, Name = name };
		}

		private static OpenXmlElement GetPreceedingElement(WorksheetPart worksheetPart)
		{
			List<Type> elements = new List<Type>()
			{
				typeof(Scenarios),
				typeof(ProtectedRanges),
				typeof(SheetProtection),
				typeof(SheetCalculationProperties),
				typeof(SheetData)
			};

			OpenXmlElement preceedingElement = null;
			foreach (var item in worksheetPart.Worksheet.ChildElements.Reverse())
			{
				if (elements.Contains(item.GetType()))
				{
					preceedingElement = item;
					break;
				}
			}
			return preceedingElement;
		}

		//private static string ReplaceHexadecimalSymbols(string txt)
		//{
		//	try
		//	{
		//		string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
		//		return Regex.Replace(txt, r, "", RegexOptions.Compiled);
		//	}
		//	catch
		//	{
		//		return txt;
		//	}
		//}

		//private static string ReplaceNotnamedSymbols(string txt)
		//{
		//	IEnumerable<string> symbols = new List<string> { "\\", ",", "?", "/", "-", ":", "*", "+" };
		//	foreach (string s in symbols)
		//		txt = txt.Replace(s, "");
		//	return txt;
		//}


		private static void SetColumnWidth(Worksheet sheet, Dictionary<int, double> ColumnsWidth)
		{
			Columns columns = sheet.GetFirstChild<Columns>();
			bool needToInsertColumns = false;
			if (columns == null)
			{
				columns = new Columns();
				needToInsertColumns = true;
			}

			foreach (int col in ColumnsWidth.Keys)
				columns.Append(new Column() { Min = (uint)col, Max = (uint)col, Width = ColumnsWidth[col], CustomWidth = true });

			if (needToInsertColumns)
				sheet.InsertAt(columns, 0);
		}



		private static SheetViews GetSheetViews(bool fixedFirstRow)
		{
			var sheetViews = new SheetViews() { };
			var sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U, ZoomScaleNormal = 100U };
			if (fixedFirstRow)
			{
				StringValue value = new StringValue("G31");
				ListValue<StringValue> values = new ListValue<StringValue>();
				values.Items.Add(value);

				sheetView.Append(GetPane());
				sheetView.Append(GetSelection(values));
			}

			sheetViews.Append(sheetView);
			return sheetViews;
		}

		private static Pane GetPane()
		{
			return new Pane()
			{
				VerticalSplit = 3D,
				TopLeftCell = "A4",
				ActivePane = PaneValues.BottomLeft,
				State = PaneStateValues.Frozen
			};
		}

		private static Selection GetSelection(ListValue<StringValue> values)
		{
			return new Selection()
			{
				Pane = PaneValues.BottomLeft,
				ActiveCell = "G31",
				SequenceOfReferences = values
			};
		}


		#region Styles

		private enum FCellFormats { Default = 0, BorderCenterBold, BorderLeftNormal, GreenBold12, DarkGreenBold1, BorderDate, BorderNumber };


		private static Stylesheet GenerateStylesheet()
		{
			return new Stylesheet(GetFonts(), GetFills(), GetBorders(), GetCellFormats());
		}

		private static CellFormats GetCellFormats()
		{
			return new CellFormats(
					  new CellFormat(), // 0 - default
					  new CellFormat // 1 border-center-bold
					  {
						  FontId = 1,
						  FillId = 0,
						  BorderId = 1,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Center,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = true
						  }
					  },
					  new CellFormat // 2 border-left-normal
					  {
						  FontId = 0,
						  FillId = 0,
						  BorderId = 1,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Left,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = true
						  }
					  },
					  new CellFormat // 3 - green bold 12
					  {
						  FontId = 2,
						  FillId = 0,
						  BorderId = 0,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Left,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = false
						  }
					  },
					  new CellFormat // 4 - dark green bold 11
					  {
						  FontId = 3,
						  FillId = 0,
						  BorderId = 0,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Left,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = false
						  }
					  },
					  new CellFormat // 5 - border date
					  {
						   FontId = 0,
						   FillId = 0,
						   BorderId = 1,
						   ApplyBorder = true,
						   Alignment = new Alignment()
						   {
							   Horizontal = HorizontalAlignmentValues.Center,
							   Vertical = VerticalAlignmentValues.Center,
							   WrapText = false
						   },
						  NumberFormatId = 22,
						  ApplyNumberFormat = true
					  },
					  new CellFormat // 5 - border number
					  {
						  FontId = 0,
						  FillId = 0,
						  BorderId = 1,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Center,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = false
						  },
						  NumberFormatId = 41,
						  ApplyNumberFormat = true
					  }
				);
		}
		private static Fonts GetFonts()
		{
			return new Fonts(
				new Font( // Index 0 - default
					new FontSize() { Val = 10 }),
				new Font( // Index 1 - black bold 10
					new FontSize() { Val = 10 },
					new Bold() { Val = true },
					new Color() { Rgb = "000000" }),
				new Font( // Index 2 - green bold 14
					new FontSize() { Val = 12 },
					new Bold() { Val = true },
					new Color() { Rgb = new HexBinaryValue() { Value = "1abc9c" } }),
				new Font( // Index 3 - dark green bold 11
					new FontSize() { Val = 11 },
					new Bold() { Val = true },
					new Color() { Rgb = new HexBinaryValue() { Value = "2e4154" } })
				);
		}
		private static Fills GetFills()
		{
			return new Fills(
					 //0 none
					 new Fill(new PatternFill()
					 {
						 PatternType = PatternValues.None
					 })
				 );
		}
		private static Borders GetBorders()
		{
			return new Borders(
					new Border(), // index 0 default
					new Border( // index 1 gray border
						new LeftBorder(new Color() { Rgb = new HexBinaryValue() { Value = "	919191" } }) { Style = BorderStyleValues.Thin },
						new RightBorder(new Color() { Rgb = new HexBinaryValue() { Value = "919191" } }) { Style = BorderStyleValues.Thin },
						new TopBorder(new Color() { Rgb = new HexBinaryValue() { Value = "919191" } }) { Style = BorderStyleValues.Thin },
						new BottomBorder(new Color() { Rgb = new HexBinaryValue() { Value = "919191" } }) { Style = BorderStyleValues.Thin },
						new DiagonalBorder()),
					new Border( // index 2 black border
						new LeftBorder(new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
						new RightBorder(new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
						new TopBorder(new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
						new BottomBorder(new Color() { Rgb = new HexBinaryValue() { Value = "000000" } }) { Style = BorderStyleValues.Thin },
						new DiagonalBorder())
				);
		}
		#endregion Styles

	}
}