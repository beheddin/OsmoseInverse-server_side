using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Data.Repositories;
using Domain.Commands;
using Domain.Handlers;
using Domain.Interfaces;
using Domain.Models;
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

            #region Compte
            services.AddTransient<IGenericRepository<Compte>, GenericRepository<Compte>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Compte>, IEnumerable<Compte>>, GetAllGenericHandler<Compte>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Compte>, Compte>, GetByGenericHandler<Compte>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Compte>, string>, PostGenericHandler<Compte>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Compte>, string>, PutGenericHandler<Compte>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Compte>, string>, DeleteGenericHandler<Compte>>();
            //services.AddTransient<IRequestHandler<DeleteObject<Compte>, string>, DeleteObjectHandler<Compte>>();
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

            #region StationEntretien
            services.AddTransient<IGenericRepository<StationEntretien>, GenericRepository<StationEntretien>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<StationEntretien>, IEnumerable<StationEntretien>>, GetAllGenericHandler<StationEntretien>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<StationEntretien>, StationEntretien>, GetByGenericHandler<StationEntretien>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<StationEntretien>, string>, PostGenericHandler<StationEntretien>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<StationEntretien>, string>, PutGenericHandler<StationEntretien>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<StationEntretien>, string>, DeleteGenericHandler<StationEntretien>>();
            #endregion

            #region SourceEau
            services.AddTransient<IGenericRepository<SourceEau>, GenericRepository<SourceEau>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<SourceEau>, IEnumerable<SourceEau>>, GetAllGenericHandler<SourceEau>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<SourceEau>, SourceEau>, GetByGenericHandler<SourceEau>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<SourceEau>, string>, PostGenericHandler<SourceEau>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<SourceEau>, string>, PutGenericHandler<SourceEau>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<SourceEau>, string>, DeleteGenericHandler<SourceEau>>();
            #endregion

            #region Puit
            services.AddTransient<IGenericRepository<Puit>, GenericRepository<Puit>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Puit>, IEnumerable<Puit>>, GetAllGenericHandler<Puit>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Puit>, Puit>, GetByGenericHandler<Puit>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Puit>, string>, PostGenericHandler<Puit>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Puit>, string>, PutGenericHandler<Puit>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Puit>, string>, DeleteGenericHandler<Puit>>();
            #endregion

            #region Bassin
            services.AddTransient<IGenericRepository<Bassin>, GenericRepository<Bassin>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Bassin>, IEnumerable<Bassin>>, GetAllGenericHandler<Bassin>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Bassin>, Bassin>, GetByGenericHandler<Bassin>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Bassin>, string>, PostGenericHandler<Bassin>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Bassin>, string>, PutGenericHandler<Bassin>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Bassin>, string>, DeleteGenericHandler<Bassin>>();
            #endregion

            #region SourceEauEntretien
            services.AddTransient<IGenericRepository<SourceEauEntretien>, GenericRepository<SourceEauEntretien>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<SourceEauEntretien>, IEnumerable<SourceEauEntretien>>, GetAllGenericHandler<SourceEauEntretien>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<SourceEauEntretien>, SourceEauEntretien>, GetByGenericHandler<SourceEauEntretien>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<SourceEauEntretien>, string>, PostGenericHandler<SourceEauEntretien>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<SourceEauEntretien>, string>, PutGenericHandler<SourceEauEntretien>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<SourceEauEntretien>, string>, DeleteGenericHandler<SourceEauEntretien>>();
            #endregion

            //#region PuitEntretien
            //services.AddTransient<IGenericRepository<PuitEntretien>, GenericRepository<PuitEntretien>>();

            //services.AddTransient<IRequestHandler<GetAllGenericQuery<PuitEntretien>, IEnumerable<PuitEntretien>>, GetAllGenericHandler<PuitEntretien>>();
            //services.AddTransient<IRequestHandler<GetByGenericQuery<PuitEntretien>, PuitEntretien>, GetByGenericHandler<PuitEntretien>>();

            //services.AddTransient<IRequestHandler<PostGenericCommand<PuitEntretien>, string>, PostGenericHandler<PuitEntretien>>();
            //services.AddTransient<IRequestHandler<PutGenericCommand<PuitEntretien>, string>, PutGenericHandler<PuitEntretien>>();
            //services.AddTransient<IRequestHandler<DeleteGenericCommand<PuitEntretien>, string>, DeleteGenericHandler<PuitEntretien>>();
            //#endregion

            //#region BassinEntretien
            //services.AddTransient<IGenericRepository<BassinEntretien>, GenericRepository<BassinEntretien>>();

            //services.AddTransient<IRequestHandler<GetAllGenericQuery<BassinEntretien>, IEnumerable<BassinEntretien>>, GetAllGenericHandler<BassinEntretien>>();
            //services.AddTransient<IRequestHandler<GetByGenericQuery<BassinEntretien>, BassinEntretien>, GetByGenericHandler<BassinEntretien>>();

            //services.AddTransient<IRequestHandler<PostGenericCommand<BassinEntretien>, string>, PostGenericHandler<BassinEntretien>>();
            //services.AddTransient<IRequestHandler<PutGenericCommand<BassinEntretien>, string>, PutGenericHandler<BassinEntretien>>();
            //services.AddTransient<IRequestHandler<DeleteGenericCommand<BassinEntretien>, string>, DeleteGenericHandler<BassinEntretien>>();
            //#endregion

            #region Fournisseur
            services.AddTransient<IGenericRepository<Fournisseur>, GenericRepository<Fournisseur>>();

            services.AddTransient<IRequestHandler<GetAllGenericQuery<Fournisseur>, IEnumerable<Fournisseur>>, GetAllGenericHandler<Fournisseur>>();
            services.AddTransient<IRequestHandler<GetByGenericQuery<Fournisseur>, Fournisseur>, GetByGenericHandler<Fournisseur>>();

            services.AddTransient<IRequestHandler<PostGenericCommand<Fournisseur>, string>, PostGenericHandler<Fournisseur>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Fournisseur>, string>, PutGenericHandler<Fournisseur>>();
            services.AddTransient<IRequestHandler<DeleteGenericCommand<Fournisseur>, string>, DeleteGenericHandler<Fournisseur>>();
            #endregion

            //#region TypeEquipment

            //#endregion
            //#region NatureParty

            //#endregion
            //#region Goal
            //#endregion

            //#region Equipment
            //#endregion

            //#region StationParametre
            //#endregion

            //#region TrackingParametre
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