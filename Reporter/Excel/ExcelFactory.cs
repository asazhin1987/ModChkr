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

		//private static string ReplaceNotnamedSymbols(string txt)
		//{
		//	IEnumerable<string> symbols = new List<string> { "\\", ",", "?", "/", "-", ":", "*", "+" };
		//	foreach (string s in symbols)
		//		txt = txt.Replace(s, "");
		//	return txt;
		//}

		private static string ReplaceMoneySymbols(string txt)
		{
			try
			{
				return txt.Replace(",", ".");
			}
			catch
			{
				return txt;
			}
		}

		private static void SetColumnWidth(Worksheet sheet, IEnumerable<double> ColumnsWidth)
		{
			Columns columns = sheet.GetFirstChild<Columns>();
			bool needToInsertColumns = false;
			if (columns == null)
			{
				columns = new Columns();
				needToInsertColumns = true;
			}

			uint c = 0;
			foreach (int col in ColumnsWidth)
			{
				c++;
				columns.Append(new Column() { Min = c, Max = c, Width = col, CustomWidth = true });
			}
				

			if (needToInsertColumns)
				sheet.InsertAt(columns, 0);
		}



		private static SheetViews GetSheetViews(bool fixedFirstRow, double verticalsplit, string topleftcell)
		{
			var sheetViews = new SheetViews() { };
			var sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U, ZoomScaleNormal = 100U };
			if (fixedFirstRow)
			{
				StringValue value = new StringValue("G31");
				ListValue<StringValue> values = new ListValue<StringValue>();
				values.Items.Add(value);

				sheetView.Append(GetPane(verticalsplit, topleftcell));
				sheetView.Append(GetSelection(values));
			}

			sheetViews.Append(sheetView);
			return sheetViews;
		}

		private static Pane GetPane(double verticalsplit, string topleftcell)
		{
			return new Pane()
			{
				VerticalSplit = verticalsplit,
				TopLeftCell = topleftcell,
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

		private static void MergeCells(MergeCells merge, string literA, string literB, uint row)
		{
			merge.Append(new MergeCell() { Reference = new StringValue($"{literA}{row}:{literB}{row}") });
		}

		private static void MergeCells(MergeCells merge, string CellA, string CellB)
		{
			merge.Append(new MergeCell() { Reference = new StringValue($"{CellA}:{CellB}") });
		}

		#region Styles

		private enum FCellFormats { Default = 0, BorderCenterBold, BorderLeftNormal, GreenBold12, DarkGreenBold1, BorderDate, BorderNumber, BorderInt, BorderDouble };


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
						  NumberFormatId = 19,
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
					  },
					  new CellFormat // 6 - border int
					  {
						  FontId = 0,
						  FillId = 0,
						  BorderId = 1,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Left,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = false
						  },
						  NumberFormatId = 1,
						  ApplyNumberFormat = true
					  },
					  new CellFormat // 7 - border double
					  {
						  FontId = 0,
						  FillId = 0,
						  BorderId = 1,
						  ApplyBorder = true,
						  Alignment = new Alignment()
						  {
							  Horizontal = HorizontalAlignmentValues.Left,
							  Vertical = VerticalAlignmentValues.Center,
							  WrapText = false
						  },
						  NumberFormatId = 4,
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
