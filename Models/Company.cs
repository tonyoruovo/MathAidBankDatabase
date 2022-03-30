using System;
using System.Collections.Generic;
using System.Globalization;
#region 
namespace DataBaseTest.Models
{
    public abstract class Company<T> : IEntity, ISubject<T>
    {

        /*
		 * the keyword params here means that this parameter is a varargs
		 */
        private static void NullCheck(string[] argNames, params object[] args)
        {
            if (argNames.Length != args.Length)
                throw new ArgumentException("1st array is not equal to 2nd array in length");
            for (int i = 0; i < args.Length; i++)
            {
                object o = args[i];
                if (o == null)
                    throw new ArgumentNullException("The argument {0} is null", argNames[i]);
            }
        }

        /// <summary>
        /// A full <c>Company</c> object with a name, head and contact inofrmation.
        /// </summary>
        /// <param name="head">The managing head of the company.</param>
        /// <param name="name">The company name.</param>
        /// <param name="contactInfo">The office location of this company.</param>
        protected Company(IEmployee head, IName name, IContactInfo contactInfo)
        {
            NullCheck(new string[] { "name", "head", "contactInfo" }, name, head, contactInfo);
            this.Name = name;
            this.Head = head;
            Branches = new List<T>();
            ContactInfo = contactInfo;
        }

        protected Company(IEmployee head, IEntity entity)
        {
            NullCheck(new string[] { "entity" }, entity);
            Head = head;
            Name = entity.Name;
            ContactInfo = entity.ContactInfo;
            Branches = new List<T>();
        }

        public IEmployee Head
        {
            protected set;
            get;
        }

        public IContactInfo ContactInfo
        {
            protected set;
            get;
        }

        ///<summary>
        ///Gets the name of the company and may private set it as well.
        ///</summary>
        ///<return>an <c>IName</c> object representing the name of this
        ///company</return>
        public IName Name
        {
            get;
        }

        public T this[int index]
        {
            get => Branches[index];
            set => Branches[index] = value;
        }

        public abstract List<IObserver<T>> GetClients();

        public abstract void Register(IObserver<T> o);

        public abstract void UnRegister(IObserver<T> o);

        public abstract void Update();

        private IList<T> Branches;
    }

    public interface IObserver<T>
    {
        void Inform(T t);

        void DoAction();
    }

    public interface ISubject<T>
    {

        /// <summary>
        /// Adds the <c>Observer</c> argument to the list
        /// of clients.
        /// </summary>
        /// <param name="o">The <c>Observer</c> to be added.</param>
        void Register(IObserver<T> o);

        /// <summary>
        /// Removes the <c>Observer</c> argument from the list
        /// of clients.
        /// </summary>
        /// <param name="o">The <c>Observer</c> to be removed.</param>
        void UnRegister(IObserver<T> o);

        ///<summary>
        ///Updates each <c>Observer</c> in the client list by calling it's
        /// <c>inform()</c> method
        /// </summary>
        void Update();
    }

    public interface IEntity
    {
        IName Name
        {
            get;
        }

        IContactInfo ContactInfo
        {
            get;
        }
    }

    public interface IContactInfo
    {
        IList<string> Emails
        {
            get;
        }

        IList<string> SocialMedia
        {
            get;
        }

        IList<uint> Mobiles
        {
            get;
        }

        IList<string> Websites
        {
            get;
        }

        IAddress Address
        {
            get;
        }
    }

    public interface IPerson : IEntity
    {
        byte Age();
        /*{
            TimeSpan age = DateTime.Now - BirthDate;
            return ((byte)(uint)(age.Days / 360));
        }*/
        IFullName Names
        {
            get; set;
        }

        Guid UniqueTag
        {
            get;
        }

        DateTime BirthDate
        {
            get;
        }

        MaritalStatus MaritalStatus
        {
            get; set;
        }

        INationality CountryOfOrigin
        {
            get;
        }

        IPerson NextOfKin
        {
            get; set;
        }

        bool IsMale
        {
            get;
        }

        /// <summary>
        /// Gets a <c>IDictionary</c> which uses string constants as keys and
        /// and <c>uint</c> as the identification number. For example an entry
        /// may contain <c>"voter"</c> as the key then followed by an <c>uint</c>
        /// as the Voter Identification Number.
        /// </summary>
        /// <remarks>Use one of the string constants in the class <c>Constants</c>
        /// to retrieve values from the <c>IDictionary</c> that is returned.</remarks>
        /// <returns>an <c>IDictionary</c> of string/uint as it's key/value pair.</returns>
        IDictionary<string, uint> Identification
        {
            get;
        }
    }

    public interface IEmployee : IPerson
    {

        ushort Level
        {
            get;
        }

        string Position
        {
            get;
        }

        string Username
        {
            get;
            set;
        }

        string Password
        {
            get; set;
        }

        IEmployee CurrentSupervisor
        {
            get;
        }

        DateTime HireDate
        {
            get;
        }

        IEmployee[] Group
        {
            get;
        }

        IEmployee Superior
        {
            get;
        }

        IEmployee Subordinate
        {
            get;
        }

        WorkingStatus WorkingStatus
        {
            get;
        }

        IEducation Qualification
        {
            get;
        }
    }

    public enum WorkingStatus
    {
        NONE = 0, ACTIVE, ON_LEAVE, ON_TRAINING, SUSPENDED, RETIRED, TERMINATED,
        ON_PROBATION
    }

    public enum MaritalStatus
    {
        SINGLE, ENGAGED, MARRIED, DIVORCED, WIDOWED
    }

    public interface IName
    {
        /// <summary>
        ///  Gets the name corresponding to this object
        /// </summary>
        /// <returns>the name of this object as a string</returns>
        string Name
        {
            get;
        }
    }

    public interface IAddress : INationality
    {
        string StreetAddress
        {
            get;
        }

        ushort ZipCode
        {
            get;
        }

        string PMB
        {
            get;
        }
    }

    public interface INationality
    {
        ///<summary>
        ///Gets the contry name
        /// </summary>
        ///<returns>
        ///the name of the country
        ///</returns>
        ///
        IName CountryName
        {
            get;
        }

        CultureInfo Language
        {
            get;
        }

        IName State
        {
            get;
        }

        String LGA
        {
            get;
        }

        IName CityTown
        {
            get;
        }
    }

    public interface IEducation
    {
        Qualification Primary
        {
            get;
        }
        Qualification Secondary
        {
            get;
        }

        Qualification PrimaryTertiary
        {
            get;
        }

        Qualification[] Others
        {
            get;
        }
    }

    public readonly struct Qualification
    {
        /// <summary>
        /// Constructs a <c>Qualification</c> with a <c>IReadOnlyDictionary</c> whereby the keys
        /// correspond to the named subject such as "Mathematics" and the grades correspond to
        /// grading values eg for W.A.S.S.C.E it may be 0 (A) and 9 (F).
        /// </summary>
        /// <param name="grades"> a <c>IReadOnlyDictionary</c> with subject represented as
        /// <c>string</c> and grading represented as <c>byte</c>.</param>
        /// <param name="certification">a <c>string</c> representing the name of the certification.
        /// For example W.A.S.S.C.E may use "W.A.S.S.C.E".</param>
        /// <param name="academyAddress">an <c>IAddress</c> object associated with the institute,
        /// college or school that presented this credential.</param>
        public Qualification(IReadOnlyDictionary<string, byte> grades, string certification, IAddress academyAddress)
        {
            this.grades = grades;
            this.Certification = certification;
            this.Academy = academyAddress;
        }

        private readonly IReadOnlyDictionary<string, byte> grades;
        public string Certification
        {
            get;
        }
        public IAddress Academy
        {
            get;
        }

        public byte getGradeFor(string course)
        {
            return grades[course];
        }
    }

    public interface IFullName : IName
    {
        /// <summary>
        /// Gets the surname of <c>this</c> as an <c>IName</c>.
        /// </summary>
        /// <return>an <c>IName</c> Representing the surname of this object.</return>
        IName Surname
        {
            get;
        }

        /// <summary>
        /// Gets the proposed middle name and any other name
        /// used for identifying this object. The value returned
        /// is stored as a an array i.e every extra name belonging to this object
        /// as an element of the returned array.
        /// </summary>
        /// <return>
        /// an array of <c>IName</c> objects with each index corresponding to a
        /// single name.
        /// </return>
        IName[] OtherNames
        {
            get;
        }

        /// <summary>
        /// Gets the title any title associated with this object.
        /// For human names, this would be "Mr" or "Mrs" or "Chief"
        /// etc.
        /// </summary>
        /// <returns>The title associated with this name as a <c>string</c></returns>
        String Title
        {
            get;
        }

        /// <summary>
        /// A property that represents the nickname which is the part of the <c>IFullName</c>
        /// surrounded by quotation(s).
        /// </summary>
        /// <returns> a <c>IName</c> object representing the nickname.</returns>
        IName Nickname
        {
            get;
        }
    }
}
#endregion
