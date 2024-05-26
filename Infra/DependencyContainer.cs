using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Data.Repositories;
using Domain.Commands;
using Domain.Handlers;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Queries;
using Domain.DataTransferObjects;
using System;
using System.Collections.Generic;
using Data.Context;

namespace Infra
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<OsmoseInverseDbContext>();

            #region User
            services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<User>, IEnumerable<User>>, GetAllGenericHandler<User>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<User>, User>, GetByGenericHandler<User>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<User>, string>, PostGenericHandler<User>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<User>, string>, PutGenericHandler<User>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<User>, string>, DeleteGenericHandler<User>>();
            //services.AddTransient<IRequestHandler<DeleteObject<User>, string>, DeleteObjectHandler<User>>();
            #endregion

            #region Role
            services.AddTransient<IGenericRepository<Role>, GenericRepository<Role>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Role>, IEnumerable<Role>>, GetAllGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Role>, Role>, GetByGenericHandler<Role>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Role>, string>, PostGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Role>, string>, PutGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Role>, string>, DeleteGenericHandler<Role>>();
            //services.AddTransient<IRequestHandler<DeleteObject<Role>, string>, DeleteObjectHandler<Role>>();
            #endregion

            #region Filiale
            services.AddTransient<IGenericRepository<Filiale>, GenericRepository<Filiale>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Filiale>, IEnumerable<Filiale>>, GetAllGenericHandler<Filiale>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Filiale>, Filiale>, GetByGenericHandler<Filiale>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Filiale>, string>, PostGenericHandler<Filiale>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Filiale>, string>, PutGenericHandler<Filiale>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Filiale>, string>, DeleteGenericHandler<Filiale>>();
            #endregion

            #region Atelier
            services.AddTransient<IGenericRepository<Atelier>, GenericRepository<Atelier>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Atelier>, IEnumerable<Atelier>>, GetAllGenericHandler<Atelier>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Atelier>, Atelier>, GetByGenericHandler<Atelier>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Atelier>, string>, PostGenericHandler<Atelier>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Atelier>, string>, PutGenericHandler<Atelier>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Atelier>, string>, DeleteGenericHandler<Atelier>>();
            #endregion

            #region Station
            services.AddTransient<IGenericRepository<Station>, GenericRepository<Station>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Station>, IEnumerable<Station>>, GetAllGenericHandler<Station>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Station>, Station>, GetByGenericHandler<Station>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Station>, string>, PostGenericHandler<Station>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Station>, string>, PutGenericHandler<Station>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Station>, string>, DeleteGenericHandler<Station>>();
            #endregion

            //#region TypeEquipment

            //#endregion
            //#region NatureParty

            //#endregion
            //#region Goal
            //#endregion

            //#region Equipment
            //#endregion

            //#region StationParameter
            //#endregion

            //#region TrackingParameter
            //#endregion

            //#region DailyTrackedItem
            //#endregion

            //#region ConsumableProduct
            //#endregion

            //#region MembraneChangement
            //#endregion

            //#region CartridgeChangement
            //#endregion

            //#region ChemicalWashing
            //#endregion

            //#region ChemicalDosage
            //#endregion

            //#region Maintain
            //#endregion

            //#region OsmoseMaintain
            //#endregion

            //#region WellMaintain
            //#endregion

            //#region WellMaintain
            //#endregion

            //#region CartridgeType
            //#endregion

            //#region ChemicalProduct
            //#endregion

            //#region ChemicalProductCategory
            //#endregion

            //#region ChekListElement
            //#endregion

            //#region Pool
            //#endregion

            //#region MembraneType
            //#endregion



            //#region Supplier
            //#endregion

            //#region TrackingType
            //#endregion

            //#region Unite
            //#endregion

            //#region Well
            //#endregion
        }
    }
}