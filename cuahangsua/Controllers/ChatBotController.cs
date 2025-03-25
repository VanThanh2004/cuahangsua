using cuahangsua.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace cuahangsua.Controllers
{
    [Route("api/chatbot")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>
        {
            { "sữa", new List<string> { "Cửa hàng có các loại sữa như: Sữa Nan, Sữa Meiji, Sữa Similac, Sữa Friso." } },
            { "Cửa hàng", new List<string> { "Cửa hàng có các sản phẩm cho bé như: Sữa, Bỉm, Quần áo, Đồ chơi." } },
            { "cửa hàng", new List<string> { "Cửa hàng có các sản phẩm cho bé như: Sữa, Bỉm, Quần áo, Đồ chơi." } },
            { "bỉm", new List<string> { "Cửa hàng có các loại bỉm: Merries, Bỉm trẻ em, Bỉm trẻ em, Bobby." } },
            { "quần áo", new List<string> { "Chúng tôi có quần áo bé như: Bộ quần áo bé gái, Váy bé gái, Bộ quần áo sơ sinh." } },
            { "đồ chơi", new List<string> { "Có các loại đồ chơi:Bộ đồ chơi xếp hình,Gấu bông dễ thương, Xe điều khiển từ xa, Búp bê Barbie." } },
            { "đặt hàng", new List<string> { "Thêm sản phẩm vào giỏ hàng và nhấn thanh toán để mua hàng." } },
            { "giao hàng", new List<string> { "Cửa hàng có hỗ trợ giao hàng tận nơi trên toàn quốc." } },
            { "phí ship", new List<string> { "Phí ship thay đổi tùy khu vực, bạn có thể kiểm tra khi đặt hàng." } },
            { "thanh toán", new List<string> { "Cửa hàng hỗ trợ thanh toán khi nhận hàng (COD) hoặc thanh toán online." } },
            { "thời gian giao", new List<string> { "Thời gian giao hàng thường từ 2-5 ngày làm việc tùy khu vực." } },
            { "đổi trả", new List<string> { "Bạn có thể đổi trả trong vòng 7 ngày nếu sản phẩm lỗi hoặc không đúng mô tả." } }
        };

        private static readonly List<string> greetings = new List<string>
        {
            "chào", "hello", "hi", "xin chào", "shop ơi"
        };

        [HttpGet("ask")]
        public IActionResult Ask(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Vui lòng nhập câu hỏi.");

            string lowerQuestion = question.ToLower();

            // Nếu khách hàng chào shop, bot trả lời chào lại
            if (greetings.Any(g => lowerQuestion.Contains(g)))
            {
                return Ok(new ChatMessage
                {
                    UserMessage = question,
                    BotResponse = "Chào bạn! 😊 Bạn cần giúp đỡ gì ạ?"
                });
            }

            // Kiểm tra xem câu hỏi chứa từ khóa nào
            foreach (var keyword in keywordResponses.Keys)
            {
                if (lowerQuestion.Contains(keyword))
                {
                    return Ok(new ChatMessage
                    {
                        UserMessage = question,
                        BotResponse = keywordResponses[keyword][0]  // Lấy câu trả lời đầu tiên phù hợp
                    });
                }
            }

            // Nếu không tìm thấy câu trả lời phù hợp
            return Ok(new ChatMessage
            {
                UserMessage = question,
                BotResponse = "Xin lỗi, tôi không hiểu câu hỏi của bạn. Vui lòng thử lại hoặc liên hệ shop để được hỗ trợ."
            });
        }
    }
}
