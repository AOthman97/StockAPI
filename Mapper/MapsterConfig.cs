using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Models;
using Mapster;

namespace api.Mapper
{
    public class MapsterConfig
    {
        public static void Configure()
        {
            // Configure Product to ProductDto mapping
            TypeAdapterConfig<Stock, StockDto>.NewConfig()

                .Map(dest => dest.Comments, src => src.Comments.Adapt<List<CommentDto>>());

            //.Map(dest => dest.lang, src => MapContext.Current.Parameters["lang"]);

            //.BuildAdapter();
            //.AddParameters("lang", "");

            //.Map(dest => dest.Symbol, src => $"{src.Symbol} - {src.Industry} - {src.CompanyName}")

            // direct list mapping
            //.Map(dest => dest.Purchase,
            //            src => src.MarketCap - src.LastDiv, // Here is the custom value that it would calculate and show
            //            srcCond => srcCond.MarketCap > 5) // Here is the condition to make the above calculation work, else print 0

            // nested objects mapping
            //.Map(dest => dest.Comments, src => src.Comments.Adapt<CommentDto>())

            //.Map(dest => dest.LastDiv, src => src.LastDiv)

            //.Map(dest => dest.Industry, src => src.Industry)

            // This is another example to choose one of two values
    //        TypeAdapterConfig<ParentPoco, ParentDto>.NewConfig()
    //        .Fork(config => config.ForType<string, string>()
    //        .MapToTargetWith((src, dest) => string.IsNullOrEmpty(src) ? dest : src)
    //        );

            //.Map(dest => dest.MarketCap, src => MapContext.Current.Parameters["lang"]);

            // Configure Review to ReviewDto mapping
            TypeAdapterConfig<Comment, CommentDto>.NewConfig()

                .Map(dest => dest.Title, src => src.Title)

                .Map(dest => dest.Content, src => src.Content)
                
                .Map(dest => dest.StockId, src => src.StockId);
        }
    }
}