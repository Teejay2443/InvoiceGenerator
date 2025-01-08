using AspNetCoreHero.ToastNotification.Abstractions;
using InvoiceGenerator.Dto;
using InvoiceGenerator.Interface.IRepositories;
using InvoiceGenerator.Interface.IServices;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Services
{
    public class AreaCoveredServices : IAreaCoveredServices
    {
        private readonly IAreaCoveredRepository _AreaCoveredRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly INotyfService _notyfService;
        public AreaCoveredServices(IAreaCoveredRepository AreaCoveredRepository, ApplicationDbContext dbContext, INotyfService notyfService)
        {
            _AreaCoveredRepository = AreaCoveredRepository;
            _dbContext = dbContext;
            _notyfService = notyfService;
        }
        public async Task<BaseResponse<bool>> CreateAreaCovered(CreateAreaCoveredDto request)
        {
            try
            {
                var newArea = new AreaCovered()
                {
                    Id = request.Id,
                    Name = request.Name,
                };
                await _dbContext.AreaCovered.AddAsync(newArea);
                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    _notyfService.Success("AreaCovered created sucessfully");
                    return new BaseResponse<bool> { Message = "AreaCovered created successfully", IsSuccessful = true, Data = true };
                }
                _notyfService.Error("AreaCovered Creation failed");
                return new BaseResponse<bool> { Message = "AreaCovered Creation failed", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
        }

        public async Task<BaseResponse<bool>> Delete(Guid Id)
        {
            try
            {
                var area = await _AreaCoveredRepository.GetAreaCoveredAsync(Id);

                if (area != null)
                {
                    _dbContext.AreaCovered.Remove(area);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        _notyfService.Success("AreaConvered deleted sucessfully");
                        return new BaseResponse<bool> { Message = "AreaCovered deleted successfully", IsSuccessful = true, Data = true };
                    }

                }
                _notyfService.Error("AreaCovered not found");
                return new BaseResponse<bool> { Message = "AreaCovered not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
            }
        }

        public async Task<BaseResponse<List<AreaCoveredDto>>> GetAllAreaCovered()
        {
            try
            {
                var area = await _AreaCoveredRepository.GetAllAreaCoveredAsync();

                if (area.Count > 0)
                {
                    var data = area.Select(x => new AreaCoveredDto
                    {
                        Id = x.Id,
                        Name = x.Name,


                    }).ToList();

                    return new BaseResponse<List<AreaCoveredDto>> { Message = "Data retrieved successfuly", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<List<AreaCoveredDto>> { Message = "No record", IsSuccessful = false, Data = new List<AreaCoveredDto>() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AreaCoveredDto>> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new List<AreaCoveredDto>() };
            }
        }

        public async Task<BaseResponse<AreaCoveredDto>> GetAreaCovered(Guid Id)
        {
            try
            {
                var area = await _AreaCoveredRepository.GetAreaCoveredAsync(Id);

                if (area != null)
                {
                    var data = new AreaCoveredDto
                    {
                        Id = area.Id,
                        Name = area.Name,

                    };
                    return new BaseResponse<AreaCoveredDto> { Message = "Data retrieved successfully", IsSuccessful = true, Data = data };
                }

                return new BaseResponse<AreaCoveredDto> { Message = "No record", IsSuccessful = false, Data = new AreaCoveredDto() };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AreaCoveredDto> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = new AreaCoveredDto() };
            }
        }

        public Task<BaseResponse<AreaCovered>> GetAreaCoveredById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<bool>> UpdateAreaCovered(Guid Id, UpdateAreaCoveredDto request)
        {
            try
            {
                var area = await _AreaCoveredRepository.GetAreaCoveredAsync(Id);

                if (area != null)
                {
                    area.Name = request.Name;


                    _dbContext.AreaCovered.Update(area);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        _notyfService.Success("AreaCovered updated sucessfully");
                        return new BaseResponse<bool> { Message = "AreaCovered updated successfully", IsSuccessful = true, Data = true };
                    }
                }
                _notyfService.Error("AreaCovered not found");
                return new BaseResponse<bool> { Message = "AreaCovered not found", IsSuccessful = false, Data = false };
            }
            catch (Exception ex)
            {

                return new BaseResponse<bool> { Message = $"Error : {ex.Message}", IsSuccessful = false, Data = false };
            }
        }
    }
}
