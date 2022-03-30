using DataBaseTest.Data;
using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public abstract class AbstractDatabaseLoadingOptions
    {
        public abstract AbstractDatabaseLoadingOptions Also { get; protected set; }
        public abstract Task LoadAsync();

        private class PersonOptions : OtherLoadingOptions<EmployeeModel, PersonModel>
        {
            public PersonOptions(ReferenceEntry<EmployeeModel, PersonModel> refer) : base(refer)
            {
                //this.from = from;this.to = to;
            }
            //private EmployeeModel from;
            //private PersonModel to;
            public override async Task LoadAsync()
            {
                await reference.Query()
                    .Include(x => Values.Contains(EmployeeDatabaseLoadingOptions.CountryOfOrigin) ? x.CountryOfOrigin : null)
                        .ThenInclude(x => x.CityTown)
                     .Include(x => Values.Contains(EmployeeDatabaseLoadingOptions.Entity) ? x.Entity : null)
                     .LoadAsync();


            }
        }
    }

    public abstract class DatabaseLoadingOptions<TLoadingOptions, TModel> : AbstractDatabaseLoadingOptions where TLoadingOptions : Enum
    {
        protected DatabaseLoadingOptions(TModel obj, DbContext c)
        {
            this.model = obj;
            this.c = c;
        }
        protected readonly TModel model;
        protected readonly DbContext c;

        public abstract List<TLoadingOptions> Values{ get; }
        public abstract List<int> Keys { get; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Unrecognised value")]
        public abstract int Id { get; }

        //public abstract DatabaseLoadingOptions<TLoadingOptions, TModelNext> Also { get; }
        //public abstract AbstractDatabaseLoadingOptions Also { get; }

        //public abstract Task LoadAsync();
    }

    public class EmployeeLoadingOptions : DatabaseLoadingOptions<EmployeeDatabaseLoadingOptions, EmployeeModel>
    {
        public EmployeeLoadingOptions(EmployeeModel e, BankDbContext db, int id) : base(e, db)
        {
            Values = new List<EmployeeDatabaseLoadingOptions>();
            Keys = new List<int>();
            Id = id;
        }

        public override List<EmployeeDatabaseLoadingOptions> Values { get; }
        public override List<int> Keys { get; }

        public override int Id { get; }
        public override AbstractDatabaseLoadingOptions Also { get; protected set; }

        public override async Task LoadAsync()
        {
            await c.Entry(model)
                .Reference(x => Values.Contains(EmployeeDatabaseLoadingOptions.Person) ? x.Person : null)
                .LoadAsync();
            await c.Entry(model)
                .Reference(x => Values.Contains(EmployeeDatabaseLoadingOptions.Secret) ? x.Secret : null)
                .LoadAsync();
            await c.Entry(model)
                .Reference(x => Values.Contains(EmployeeDatabaseLoadingOptions.Guarantor) ? x.Guarantor : null)
                .LoadAsync();
            await c.Entry(model)
                .Reference(x => Values.Contains(EmployeeDatabaseLoadingOptions.Qualification) ? x.Qualification : null)
                .LoadAsync();
            Values.Remove(EmployeeDatabaseLoadingOptions.Person);
            Values.Remove(EmployeeDatabaseLoadingOptions.Secret);
            Values.Remove(EmployeeDatabaseLoadingOptions.Guarantor);
            Values.Remove(EmployeeDatabaseLoadingOptions.Qualification);

            await Also.LoadAsync();
        }
    }

    public class OtherLoadingOptions<TFrom, TTo> : AbstractDatabaseLoadingOptions where TFrom : class where TTo : class
    {
        public OtherLoadingOptions(ReferenceEntry<TFrom, TTo> refer)
        {
            //this.db = db;
            this.reference = refer;
        }

        //protected readonly BankDbContext db;
        protected readonly ReferenceEntry<TFrom, TTo> reference;

        public List<EmployeeDatabaseLoadingOptions> Values { get; }
        public override AbstractDatabaseLoadingOptions Also { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override Task LoadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
