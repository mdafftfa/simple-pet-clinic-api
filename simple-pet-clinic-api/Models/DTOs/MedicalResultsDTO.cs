using System.Collections.Generic;

namespace simple_pet_clinic_api.Models.DTOs;

public class MedicalHistory
{
    public required string ChiefComplaint { get; set; }
    public required List<string> PastMedicalHistory { get; set; }
    public required List<string> VaccinationHistory { get; set; }
}

public class PhysicalExamination
{
    public required string VitalSigns { get; set; }
    public required string BodySystemExamination { get; set; }
}

public class DiagnosisResults
{
    public required string BloodTest { get; set; }
    public required string SupportingInvesti { get; set; }
}

public class Assessment
{
    public required List<string> DifferentialDiagnosis { get; set; }
    public required string DefinitiveDiagnosis { get; set; }
}

public class PlanOrTreatment
{
    public required string ClinicalTherapy { get; set; }
    public required string HomeMedication { get; set; }
    public required string SurgicalProcedures { get; set; }
    public required string InformedConsentStatus { get; set; }
}

public class ProgressNotes
{
    public required string HospitalizationChart { get; set; }
    public required string FollowUp { get; set; }
}

public class MedicalResultsDTO
{
    public required MedicalHistory MedicalHistory { get; set; }
    public required PhysicalExamination PhysicalExamination { get; set; }
    public required DiagnosisResults DiagnosisResults { get; set; }
    public required Assessment Assessment { get; set; }
    public required PlanOrTreatment PlanOrTreatment { get; set; }
    public required ProgressNotes ProgressNotes { get; set; }
}