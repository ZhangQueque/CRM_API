﻿using AutoMapper;
using BT.Data.Entities;
using BT.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.API.AutoMap
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {                 //  源    目的地
            CreateMap<Employees, EmployeeInfoDto>();

            CreateMap<EmployeeInfoDto, Employees>();

            CreateMap<CustomerCreateDto, Customers>();

                        //  源    目的地
            CreateMap<Customers, CustomerShowDto>()
                .ForMember(m=>m.CreateTime  //CustomerShowDto的时间（字符串）

                //Customers的时间（datetime）
                , config =>config.MapFrom(m=>m.CreateTime.ToLongDateString()+ m.CreateTime.ToLongTimeString()));

            CreateMap<Roles, RolesShowDto>()
               .ForMember(m => m.CreateTime  //CustomerShowDto的时间（字符串）

               //Customers的时间（datetime）
               , config => config.MapFrom(m => m.CreateTime.ToLongDateString() + m.CreateTime.ToLongTimeString()));

            CreateMap<Customers, Customers>();
            CreateMap<OrderCreateDto, Orders>();
            
            CreateMap<Products, Orders_Products>();

            CreateMap<Permissions, PermissionsDto>();


            CreateMap<PermissionsDto, PermissionsDto>();

            CreateMap<Roles, RolesShowDto>();
            CreateMap<RoleCreateDto, Roles>();
            CreateMap<PermissionCreateDto, Permissions>();
            CreateMap<PermissionsDto, PermissionsDto>();

            CreateMap<Employees_Roles, Employees_RolesShowDto>()
                .ForMember(m => m.CreateTime  //CustomerShowDto的时间（字符串）
                 
                //Customers的时间（datetime）
                , config => config.MapFrom(m => m.CreateTime.ToLongDateString() + m.CreateTime.ToLongTimeString()));


            CreateMap<Employees, EmployeeShowDto>()
              .ForMember(m => m.CreateTime  //CustomerShowDto的时间（字符串）

              //Customers的时间（datetime）
              , config => config.MapFrom(m => m.CreateTime.ToLongDateString() + m.CreateTime.ToLongTimeString()));

            CreateMap<EmployeeCreateDto, Employees>();


            CreateMap<EmployeeExcelDto, Employees>();

        }
    }
}
