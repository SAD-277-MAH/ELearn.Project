using AutoMapper;
using ELearn.Data.Context;
using ELearn.Data.Dtos.Site.Setting;
using ELearn.Data.Models;
using ELearn.Presentation.Routes.V1;
using ELearn.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELearn.Presentation.Controllers.Site.V1.Common
{
    [ApiExplorerSettings(GroupName = "v1_Site")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IUnitOfWork<DatabaseContext> _db;
        private readonly IMapper _mapper;

        public SettingsController(IUnitOfWork<DatabaseContext> db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(ApiV1Routes.Setting.GetSiteSetting)]
        public async Task<IActionResult> GetSiteSetting()
        {
            var setting = (await _db.SettingRepository.GetAsync()).LastOrDefault();

            if (setting == null)
            {
                setting = new Setting();
                await _db.SettingRepository.AddAsync(setting);
                await _db.SaveAsync();
            }

            var siteSetting = _mapper.Map<SettingForSiteDto>(setting);

            return Ok(siteSetting);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(ApiV1Routes.Setting.GetMessageSenderSetting)]
        public async Task<IActionResult> GetMessageSenderSetting()
        {
            var setting = (await _db.SettingRepository.GetAsync()).LastOrDefault();

            if (setting == null)
            {
                setting = new Setting();
                await _db.SettingRepository.AddAsync(setting);
                await _db.SaveAsync();
            }

            var siteSetting = _mapper.Map<SettingForMessageSenderDto>(setting);

            return Ok(siteSetting);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut(ApiV1Routes.Setting.UpdateSiteSetting)]
        public async Task<IActionResult> UpdateSiteSetting(SettingForSiteDto dto)
        {
            var setting = (await _db.SettingRepository.GetAsync()).LastOrDefault();

            if (setting == null)
            {
                setting = new Setting();
                await _db.SettingRepository.AddAsync(setting);
                await _db.SaveAsync();
            }

            _mapper.Map(dto,setting);

            _db.SettingRepository.Update(setting);

            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("خطا در ثبت تغییرات");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut(ApiV1Routes.Setting.UpdateMessageSenderSetting)]
        public async Task<IActionResult> UpdateMessageSenderSetting(SettingForMessageSenderDto dto)
        {
            var setting = (await _db.SettingRepository.GetAsync()).LastOrDefault();

            if (setting == null)
            {
                setting = new Setting();
                await _db.SettingRepository.AddAsync(setting);
                await _db.SaveAsync();
            }

            _mapper.Map(dto, setting);

            _db.SettingRepository.Update(setting);

            if (await _db.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest("خطا در ثبت تغییرات");
            }
        }
    }
}
