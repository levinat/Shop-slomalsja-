﻿using System.Net;
using Nancy.Json;
using Shop.Core.ServiceInterface;
using Shop.Core.Dtos.Weather;
using Shop.Core.Dto.Weather;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        public async Task<WeatherResultDto> WeatherDetail(WeatherResultDto dto)
        {
            string apikey = "wvXzY2ev4SxvtuulTE67P9W9eqQ5AkFf";
            var url = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/127964?apikey=wvXzY2ev4SxvtuulTE67P9W9eqQ5AkFf";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                WeatherRootDto weatherInfo = (new JavaScriptSerializer()).Deserialize<WeatherRootDto>(json);
                HeadlineDto headlineInfo = (new JavaScriptSerializer()).Deserialize<HeadlineDto>(json);

                WeatherResultDto result = new WeatherResultDto();

                result.EffectiveDate = weatherInfo.Headline.EffectiveDate;
                result.EffectiveEpochDate = weatherInfo.Headline.EffectiveEpochDate;
                result.Severity = weatherInfo.Headline.Severity;
                result.Text = weatherInfo.Headline.Text;
                result.Category = weatherInfo.Headline.Category;
                result.EndDate = weatherInfo.Headline.EndDate;
                result.EndEpochDate = weatherInfo.Headline.EndEpochDate;
                result.MobileLink = weatherInfo.Headline.MobileLink;
                result.Link = weatherInfo.Headline.Link;
                result.DailyForecastsDate = weatherInfo.DailyForecasts.Date;
                result.DailyForecastsEpochDate = weatherInfo.DailyForecasts.EpochDate;
                result.TempMinValue = weatherInfo.DailyForecasts.Temperature.Minimum.Value;
                result.TempMinUnit = weatherInfo.DailyForecasts.Temperature.Minimum.Unit;
                result.TempMinUnitType = weatherInfo.DailyForecasts.Temperature.Minimum.UnitType;

                var jsonString = new JavaScriptSerializer().Serialize(result);

                return jsonString;
>>>>>>> e9ca39bb0a5edc9288b8ec0a68689bd10a73f839
            }
            return dto;
        }
    }
}