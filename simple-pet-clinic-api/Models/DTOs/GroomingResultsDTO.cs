using System.Collections.Generic;

namespace simple_pet_clinic_api.Models.DTOs;

public class PreGroomingAssessment
{
    public required string FurCondition { get; set; }
    public required string Parasite { get; set; }
    public required string SkinProblems { get; set; }
    public required string EarCondition { get; set; }
    public required string EyesCondition { get; set; }
    public required string NailCondition { get; set; }
}

public class GroomingActions
{
    public required string BathType { get; set; }
    public required List<string> AdditionalTreatment { get; set; }
    public required string ShavingStyle { get; set; }
}

public class BehavioralNotes
{
    public required string PetBehaviorDuringGrooming { get; set; }
    public required string SafetyEquipmentUsed { get; set; }
}

public class GroomingResultsDTO
{
    public required PreGroomingAssessment PreGroomingAssessment { get; set; }
    public required GroomingActions GroomingActions { get; set; }
    public required BehavioralNotes BehavioralNotes { get; set; }
    public required string Recommendation { get; set; }
}