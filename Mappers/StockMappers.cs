using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        // General DTO for get and alike operations
        //public static StockDto ToStockDto(this Stock StockModel)
        //{
        //    return new StockDto
        //    {
        //        Id = StockModel.Id,
        //        Symbol = StockModel.Symbol,
        //        CompanyName = StockModel.CompanyName,
        //        Purchase = StockModel.Purchase,
        //        LastDiv = StockModel.LastDiv,
        //        Industry = StockModel.Industry,
        //        MarketCap = StockModel.MarketCap,
        //        Comments = StockModel.Comments.Select(c => c.ToCommentDto()).ToList()
        //    };
        //}

        //// Revert the DTO to the Stock for the create stock without the ID, Because EF won't allow to submit a DTO instead
        //// of a class for saving
        //public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockRequestDto)
        //{
        //    return new Stock
        //    {
        //        Symbol = stockRequestDto.Symbol,
        //        CompanyName = stockRequestDto.CompanyName,
        //        Purchase = stockRequestDto.Purchase,
        //        LastDiv = stockRequestDto.LastDiv,
        //        Industry = stockRequestDto.Industry,
        //        MarketCap = stockRequestDto.MarketCap
        //    };
        //}
    }
}
