using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Healthcare;
using ClearCanvas.Healthcare.Brokers;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Services.Admin;

namespace ClearCanvas.Ris.Application.Services
{
    public class PatientProfileAssembler
    {
        public PatientProfileSummary CreatePatientProfileSummary(PatientProfile profile, IPersistenceContext context)
        {
            PersonNameAssembler nameAssembler = new PersonNameAssembler();
            HealthcardAssembler healthcardAssembler = new HealthcardAssembler();

            PatientProfileSummary summary = new PatientProfileSummary();
            summary.Mrn = new MrnDetail(profile.Mrn.Id, profile.Mrn.AssigningAuthority);
            summary.DateOfBirth = profile.DateOfBirth;
            summary.Healthcard = healthcardAssembler.CreateHealthcardDetail(profile.Healthcard);
            summary.Name = nameAssembler.CreatePersonNameDetail(profile.Name);
            summary.PatientRef = profile.Patient.GetRef();
            summary.ProfileRef = profile.GetRef();
            summary.Sex = EnumUtils.GetEnumValueInfo<Sex>(profile.Sex, context);

            return summary;
        }

        public PatientProfileDetail CreatePatientProfileDetail(PatientProfile profile, IPersistenceContext context)
        {
            return CreatePatientProfileDetail(profile, context, true, true, true, true, true);
        }

        public PatientProfileDetail CreatePatientProfileDetail(PatientProfile profile, 
            IPersistenceContext context, 
            bool includeAddresses,
            bool includeContactPersons,
            bool includeEmailAddresses,
            bool includeTelephoneNumbers,
            bool includeNotes)
        {
            PatientProfileDetail detail = new PatientProfileDetail();

            detail.Mrn = new MrnDetail(profile.Mrn.Id, profile.Mrn.AssigningAuthority);

            HealthcardAssembler healthcardAssembler = new HealthcardAssembler();
            detail.Healthcard = healthcardAssembler.CreateHealthcardDetail(profile.Healthcard);

            PersonNameAssembler nameAssembler = new PersonNameAssembler();
            detail.Name = nameAssembler.CreatePersonNameDetail(profile.Name);
            detail.Sex = EnumUtils.GetEnumValueInfo<Sex>(profile.Sex, context);
            detail.DateOfBirth = profile.DateOfBirth;
            detail.DeathIndicator = profile.DeathIndicator;
            detail.TimeOfDeath = profile.TimeOfDeath;
            detail.PrimaryLanguage = EnumUtils.GetEnumValueInfo(profile.PrimaryLanguage);
            detail.Religion = EnumUtils.GetEnumValueInfo(profile.Religion);

            AddressAssembler addressAssembler = new AddressAssembler();
            detail.CurrentHomeAddress = addressAssembler.CreateAddressDetail(profile.CurrentHomeAddress, context);
            detail.CurrentWorkAddress = addressAssembler.CreateAddressDetail(profile.CurrentWorkAddress, context);

            TelephoneNumberAssembler telephoneAssembler = new TelephoneNumberAssembler();
            detail.CurrentHomePhone = telephoneAssembler.CreateTelephoneDetail(profile.CurrentHomePhone, context);
            detail.CurrentWorkPhone = telephoneAssembler.CreateTelephoneDetail(profile.CurrentWorkPhone, context);

            if (includeTelephoneNumbers)
            {
                detail.TelephoneNumbers = new List<TelephoneDetail>();
                foreach (TelephoneNumber t in profile.TelephoneNumbers)
                {
                    detail.TelephoneNumbers.Add(telephoneAssembler.CreateTelephoneDetail(t, context));
                }
            }

            if (includeAddresses)
            {
                detail.Addresses = new List<AddressDetail>();
                foreach (Address a in profile.Addresses)
                {
                    detail.Addresses.Add(addressAssembler.CreateAddressDetail(a, context));
                }
            }

            if (includeContactPersons)
            {
                ContactPersonAssembler contactPersonAssembler = new ContactPersonAssembler();
                detail.ContactPersons = new List<ContactPersonDetail>();
                foreach (ContactPerson cp in profile.ContactPersons)
                {
                    detail.ContactPersons.Add(contactPersonAssembler.CreateContactPersonDetail(cp, context));
                }
            }

            if (includeEmailAddresses)
            {
                EmailAddressAssembler emailAssembler = new EmailAddressAssembler();
                detail.EmailAddresses = new List<EmailAddressDetail>();
                foreach (EmailAddress e in profile.EmailAddresses)
                {
                    detail.EmailAddresses.Add(emailAssembler.CreateEmailAddressDetail(e, context));
                }
            }

            if (includeNotes)
            {
                NoteAssembler noteAssembler = new NoteAssembler();
                detail.Notes = new List<NoteDetail>();
                foreach (Note n in profile.Patient.Notes)
                {
                    detail.Notes.Add(noteAssembler.CreateNoteDetail(n, context));
                }
            }

            return detail;
        }

        public void UpdatePatientProfile(PatientProfile profile, PatientProfileDetail detail, IPersistenceContext context)
        {
            profile.Mrn.Id = detail.Mrn.Id;
            profile.Mrn.AssigningAuthority = detail.Mrn.AssigningAuthority;

            profile.Healthcard.Id = detail.Healthcard.Id;
            profile.Healthcard.AssigningAuthority = detail.Healthcard.AssigningAuthority;
            profile.Healthcard.VersionCode = detail.Healthcard.VersionCode;
            profile.Healthcard.ExpiryDate = detail.Healthcard.ExpiryDate;

            PersonNameAssembler nameAssembler = new PersonNameAssembler();
            nameAssembler.UpdatePersonName(detail.Name, profile.Name);

            profile.Sex = EnumUtils.GetEnumValue<Sex>(detail.Sex);
            profile.DateOfBirth = detail.DateOfBirth.Value;
            profile.DeathIndicator = detail.DeathIndicator;
            profile.TimeOfDeath = detail.TimeOfDeath;

            profile.PrimaryLanguage = EnumUtils.GetEnumValue<SpokenLanguageEnum>(detail.PrimaryLanguage, context);
            profile.Religion = EnumUtils.GetEnumValue<ReligionEnum>(detail.Religion, context);

            TelephoneNumberAssembler telephoneAssembler = new TelephoneNumberAssembler();
            profile.TelephoneNumbers.Clear();
            foreach (TelephoneDetail t in detail.TelephoneNumbers)
            {
                profile.TelephoneNumbers.Add(telephoneAssembler.CreateTelephoneNumber(t));
            }

            AddressAssembler addressAssembler = new AddressAssembler();
            profile.Addresses.Clear();
            foreach (AddressDetail a in detail.Addresses)
            {
                profile.Addresses.Add(addressAssembler.CreateAddress(a));
            }

            ContactPersonAssembler contactAssembler = new ContactPersonAssembler();
            profile.ContactPersons.Clear();
            foreach (ContactPersonDetail cp in detail.ContactPersons)
            {
                profile.ContactPersons.Add(contactAssembler.CreateContactPerson(cp));
            }

            EmailAddressAssembler emailAssembler = new EmailAddressAssembler();
            profile.EmailAddresses.Clear();
            foreach (EmailAddressDetail e in detail.EmailAddresses)
            {
                profile.EmailAddresses.Add(emailAssembler.CreateEmailAddress(e));
            }

            NoteAssembler noteAssembler = new NoteAssembler();
            profile.Patient.Notes.Clear();
            foreach (NoteDetail n in detail.Notes)
            {
                profile.Patient.Notes.Add(noteAssembler.CreateNote(n, context));
            }
        }
    }
}
