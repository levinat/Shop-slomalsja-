﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Dtos;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Spaceship;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly ISpaceshipService _spaceshipService;
        private readonly IFileServices _fileServices;

        public SpaceshipController
            (
            ShopDbContext context,
            ISpaceshipService spaceshipService,
            IFileServices fileServices
            )
        {
            _context = context;
            _spaceshipService = spaceshipService;
            _fileServices = fileServices;
        } 

        //ListItem
        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Spaceship
                .OrderByDescending(y => y.CreatedAt)
                .Select(x => new SpaceshipListItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Model = x.Model,
                    Company = x.Company,
                    EnginePower = x.EnginePower,
                    Country = x.Country

                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SpaceshipViewModel model = new SpaceshipViewModel();

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpaceshipViewModel model)
        {
            var dto = new SpaceshipDto()
            {
                Id = model.Id,
                Name = model.Name,
                Model = model.Model,
                Company = model.Company,
                EnginePower = model.EnginePower,
                LaunchDate = model.LaunchDate,
                CreatedAt = model.CreatedAt,
                ModifieAt = model.ModifieAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths.Select(x => new ExistingFilePathDto
                {
                    Id = x.PhotoId,
                    ExistingFilePath = x.FilePath,
                    SpaceshipId = x.SpaceshipId
                }).ToArray()
            };

            var result = await _spaceshipService.Add(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.Delete(id);
            if (spaceship == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var spaceship = await _spaceshipService.GetAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var photos = await _context.ExistingFilePath
                .Where(x => x.ProductId == id)
                .Select(y => new ExistingFilePathViewModel
                {
                    FilePath = y.FilePath,
                    PhotoId = y.Id
                })
                .ToArrayAsync();

            var model = new SpaceshipViewModel();

            model.Id = spaceship.Id;
            model.Name = spaceship.Name;
            model.Model = spaceship.Model;
            model.Company = spaceship.Company;
            model.EnginePower = spaceship.EnginePower;
            model.Country = spaceship.Country;
            model.LaunchDate = spaceship.LaunchDate;
            model.ModifieAt = spaceship.ModifieAt;
            model.CreatedAt = spaceship.CreatedAt;
            model.ExistingFilePaths.AddRange(photos);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SpaceshipViewModel model)
        {
            var dto = new SpaceshipDto()
            {
                Id = model.Id,
                Name = model.Name,
                Model = model.Model,
                Company = model.Company,
                EnginePower =model.EnginePower,
                Country = model.Country,
                LaunchDate = model.LaunchDate,
                ModifieAt = model.ModifieAt,
                CreatedAt = model.CreatedAt,
                Files = model.Files,
                ExistingFilePaths = model.ExistingFilePaths
                .Select(x => new ExistingFilePathDto
                {
                    Id = x.PhotoId,
                    ExistingFilePath = x.FilePath,
                    SpaceshipId = x.SpaceshipId
                }).ToArray()
            };

            var result = await _spaceshipService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), model);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveImage(ExistingFilePathViewModel model)
        {
            var dto = new ExistingFilePathDto()
            {
                Id = model.PhotoId
            };

            var photo = await _fileServices.RemoveImage(dto);
            if (photo == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}