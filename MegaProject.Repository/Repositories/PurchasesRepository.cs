using System.Drawing;
using MegaProject.Domain.Models;
using MegaProject.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MegaProject.Repository.Repositories;

public class PurchasesRepository : IPurchasesRepository
{
    private readonly AppDbContext _context;

    public PurchasesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Purchase> Create(Purchase entity)
    {
        await _context.Purchases.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Purchase> GetById(int id)
    {
        return await _context.Purchases.AsNoTracking()
            .Include(w => w.Material)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<List<Purchase>> Get()
    {
        return await _context.Purchases.AsNoTracking().
            Include(w => w.Material).ToListAsync();    
    }

    public async Task<byte[]> GetSmeta()
    {
        var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets
            .Add("Смета");
        
        sheet.Cells.Style.Font.Name = "Times New Roman";
        sheet.Cells.Style.Font.Size = 14;
        
        var titleCell = sheet.Cells["A1:D1"];
        titleCell.Merge = true;
        titleCell.Value = "Расчет количества закупок материалов";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.Size = 20;
        titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        sheet.Cells["A2"].Value = "Наименование";
        sheet.Cells["B2"].Value = "ед. измерения";
        sheet.Cells["C2"].Value = "кол-во";
        sheet.Cells["D2"].Value = "Дата закупки";
        
        sheet.Cells["A2:D2"].Style.Font.Bold = true;
        sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        sheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        var materialsHeader = sheet.Cells["A3:D3"];
        materialsHeader.Merge = true;
        materialsHeader.Value = "Материалы";
        materialsHeader.Style.Font.Bold = true;
        materialsHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        materialsHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        materialsHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        
        var startRow = 4;
        var purchases = await Get();
        
        if (purchases != null && purchases.Count != 0)
        {
            foreach (var purchase in purchases)
            {
                sheet.Cells[startRow, 1].Value = purchase.Material.Name;
                sheet.Cells[startRow, 2].Value = purchase.Material.MeasurementUnit;
                sheet.Cells[startRow, 3].Value = purchase.PurchaseQuantity;
                sheet.Cells[startRow, 4].Value = purchase.DateOfPurchase.ToShortDateString();
                sheet.Cells[startRow, 4].Style.Numberformat.Format = "dd.MM.yyyy";
                
                startRow++;
            }

            // Добавляем границы для данных
            var dataRange = sheet.Cells[4, 1, startRow - 1, 4];
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }
        else
        {
            sheet.Cells["A4"].Value = "Нет данных о закупках";
            sheet.Cells["A4:D4"].Merge = true;
        }
        
        var startTotalRow = 3;

        // Группируем закупки по материалам
        var materialTotals = purchases?
            .GroupBy(p => new { p.Material.Name, p.Material.MeasurementUnit })
            .Select(g => new {
                Name = g.Key.Name,
                Unit = g.Key.MeasurementUnit,
                Total = g.Sum(p => p.PurchaseQuantity)
            })
            .ToList();

        // Заголовок "Итого"
        var totalHeader = sheet.Cells[startTotalRow, 6, startTotalRow, 8];
        totalHeader.Merge = true;
        totalHeader.Value = "Итого";
        totalHeader.Style.Font.Bold = true;
        totalHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        totalHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
        totalHeader.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        startTotalRow++;

        // Заполняем итоговые данные
        if (materialTotals != null && materialTotals.Count != 0)
        {
            foreach (var item in materialTotals)
            {
                sheet.Cells[startTotalRow, 6].Value = item.Name;
                sheet.Cells[startTotalRow, 7].Value = item.Unit;
                sheet.Cells[startTotalRow, 8].Value = item.Total;
                
                // Форматируем как жирный
                sheet.Cells[startTotalRow, 6, startTotalRow, 8].Style.Font.Bold = true;
                
                startTotalRow++;
            }
        }
        else
        {
            sheet.Cells[startTotalRow, 6].Value = "Нет данных для итогов";
            sheet.Cells[startTotalRow, 7, startTotalRow, 8].Merge = true;
        }

        // Автоподбор ширины столбцов
        sheet.Cells.AutoFitColumns();
        
        return package.GetAsByteArray();
    }

    public async Task<bool> Delete(int id)
    {
        var purchase = await _context.Purchases.FindAsync(id);
        if (purchase == null)
        {
            return false;
        }

        await _context.Purchases.Where(w => w.Id == id).Include(e => e.Material).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> Update(Purchase entity)
    {
        var result = await _context.Purchases
            .Where(w => w.Id == entity.Id)
            .Include(w => w.Material)
            .ExecuteUpdateAsync(w => w
                .SetProperty(w => w.DateOfPurchase, entity.DateOfPurchase)
                .SetProperty(w => w.PurchaseQuantity, entity.PurchaseQuantity)
                .SetProperty(w => w.MaterialId, entity.MaterialId));

        return result > 0;
    }
}