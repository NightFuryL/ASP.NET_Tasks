using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Classwork_10_10_2026.Data;
using Classwork_10_10_2026.Data.Models;
using Classwork_10_10_2026.DTO;

namespace Classwork_10_10_2026.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /*
         ProductDTO - відпровідає за поверненя об'єкту за конретними полями(що б не повертати модель, інакше юрез може бачити данні які йому не потрібні)
         CreateProductDTO - відповідає за створення нового об'єкту(деякі поля мають створюватися автоматично, наприклад Id, CreatedDate, IsDeleted, а не користувачем)
         Product - модель яка відповідає за таблицю в базі данних
         */

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto); // ковертуємо CreateProductDTO у тип Product для того щоб можна було додати його в базу данних

            _context.Products.Add(product);// додаємо новий об'єкт в базу данних

            await _context.SaveChangesAsync(); // зберігаємо зміни в базі данних

            return _mapper.Map<ProductDTO>(product);// повертаємо об'єкт ProductDTO який відповідає за повернення об'єкту за конретними полями


        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            //var product = _context.Products.Find(id);

            if (product == null) return false;

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.Where(p => !p.IsDeleted).AsNoTracking().ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return _mapper.Map<ProductDTO>(product);

        }
    }
}
