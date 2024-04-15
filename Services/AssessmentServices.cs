using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models.UsersModels;
using SEP_Web.Models.AssessmentsModels;

namespace SEP_Web.Services;

public class AssessmentServices : IAssessmentServices
{
    private readonly ILogger<IAssessmentServices> _logger;
    private readonly IUserEvaluatorServices _evaluatorServices;
    private readonly IUserAdministratorServices _administratorServices;
    private readonly SEP_WebContext _database;
    private readonly IHttpContextAccessor _httpContext;

    public AssessmentServices(ILogger<IAssessmentServices> logger, IUserEvaluatorServices evaluatorServices, IUserAdministratorServices administratorServices, SEP_WebContext database, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _evaluatorServices = evaluatorServices;
        _administratorServices = administratorServices;
        _database = database;
        _httpContext = httpContext;
    }

    public async Task<Assessment> RegisterAssessments(Assessment assessment, CivilServant servant)
    {
        try
        {
            if (assessment == null)
                throw new ArgumentNullException(nameof(assessment), ExceptionMessages.ErrorArgumentNullException);

            DateTime startDate = servant.AdmissionDate;
            int[] phaseDays = { 210, 420, 630, 840, 1040 };

            for (int stage = 1; stage <= 5; stage++)
            {
                assessment = new Assessment
                {
                    Stats = AssessmentStatsEnum.NOT_EVALUATED,
                    Phase = stage,
                    StartEvaluationPeriod = startDate.AddDays(phaseDays[stage - 1]),
                    CivilServantId = servant.Id,
                    UserEvaluatorId1 = servant.UserEvaluatorId1,
                    UserEvaluatorId2 = servant.UserEvaluatorId2,
                    EvaluatedFor = null,
                    RegisterDate = DateTime.Now,
                    FinishEvaluate = startDate.AddDays(1040)
                };

                await _database.Assessments.AddAsync(assessment);
            }

            await _database.SaveChangesAsync();

            return assessment;

        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ASSESSMENT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ASSESSMENT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            assessment = null;
            return assessment;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ASSESSMENT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[ASSESSMENT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            assessment = null;
            return assessment;
        }
    }

    public async Task<Assessment> Assess(Assessment assess)
    {
        try
        {
            int userType = Convert.ToInt32(_httpContext.HttpContext.Session.GetInt32("userType"));
            int idUser = Convert.ToInt32(_httpContext.HttpContext.Session.GetInt32("userId"));

            Assessment assessmentEdit = SearchForId(assess.Id) ?? throw new ArgumentNullException(nameof(assess), ExceptionMessages.ErrorArgumentNullException);

            assessmentEdit.Stats = AssessmentStatsEnum.EVALUATED;
            assessmentEdit.EndEvaluationPeriod = DateTime.Now;

            // Atualizar propriedades
            var propertiesToUpdate = new List<string>
            {
                "Crit1_Clau1", "Jus_Crit1_Clau1",
                "Crit2_Clau1", "Jus_Crit2_Clau1", "Crit2_Clau2", "Jus_Crit2_Clau2", "Crit2_Clau3", "Jus_Crit2_Clau3", "Crit2_Clau4", "Jus_Crit2_Clau4", "Crit2_Clau5", "Jus_Crit2_Clau5",
                "Crit3_Clau1", "Jus_Crit3_Clau1", "Crit3_Clau2", "Jus_Crit3_Clau2", "Crit3_Clau3", "Jus_Crit3_Clau3", "Crit3_Clau4", "Jus_Crit3_Clau4", "Crit3_Clau5", "Jus_Crit3_Clau5",
                "Crit4_Clau1", "Jus_Crit4_Clau1", "Crit4_Clau2", "Jus_Crit4_Clau2", "Crit4_Clau3", "Jus_Crit4_Clau3", "Crit4_Clau4", "Jus_Crit4_Clau4", "Crit4_Clau5", "Jus_Crit4_Clau5",
                "Crit5_Clau1", "Jus_Crit5_Clau1", "Crit5_Clau2", "Jus_Crit5_Clau2", "Crit5_Clau3", "Jus_Crit5_Clau3", "Crit5_Clau4", "Jus_Crit5_Clau4", "Crit5_Clau5", "Jus_Crit5_Clau5",
                "Crit6_Clau1", "Jus_Crit6_Clau1", "Crit6_Clau2", "Jus_Crit6_Clau2", "Crit6_Clau3", "Jus_Crit6_Clau3", "Crit6_Clau4", "Jus_Crit6_Clau4", "Crit6_Clau5", "Jus_Crit6_Clau5",
                "Crit7_Clau1", "Jus_Crit7_Clau1", "Crit7_Clau2", "Jus_Crit7_Clau2", "Crit7_Clau3", "Jus_Crit7_Clau3", "Crit7_Clau4", "Jus_Crit7_Clau4", "Crit7_Clau5", "Jus_Crit7_Clau5",
                "Crit8_Clau1", "Jus_Crit8_Clau1",
                "Crit9_Clau1", "Jus_Crit9_Clau1", "Crit9_Clau2", "Jus_Crit9_Clau2", "Crit9_Clau3", "Jus_Crit9_Clau3", "Crit9_Clau4", "Jus_Crit9_Clau4", "Crit9_Clau5", "Jus_Crit9_Clau5",
                "MedicalRestriction", "Crit10_Justification", "ForwardingDate"
            };

            foreach (var propertyName in propertiesToUpdate)
            {
                var assessProperty = typeof(Assessment).GetProperty(propertyName);
                if (assessProperty != null)
                {
                    object value = assessProperty.GetValue(assess);
                    typeof(Assessment).GetProperty(propertyName)?.SetValue(assessmentEdit, value);
                }
            }

            // Calcular totais e médias
            var criterias = Enumerable.Range(1, 9).Select(i => new
            {
                TotProperty = $"Tot_Crit{i}",
                AvgProperty = $"Average_Crit{i}",
                Total = i == 1 || i == 8
                    ? Convert.ToInt32(assess.GetType().GetProperty($"Crit{i}_Clau1").GetValue(assess)) // Se for Crit1_Clau1 ou Crit8_Clau1, manter como int
                    : assess.GetType().GetProperties().Where(p => p.Name.StartsWith($"Crit{i}_Clau")).Sum(p => Convert.ToInt32(p.GetValue(assess))), // Para os outros, somar e converter para int
            });

            foreach (var crit in criterias)
            {
                typeof(Assessment).GetProperty(crit.TotProperty)?.SetValue(assessmentEdit, crit.Total);

                // Verificar se é Crit1_Clau1 ou Crit8_Clau1 para manter o valor original
                if (crit.TotProperty == "Tot_Crit1" || crit.TotProperty == "Tot_Crit8")
                {
                    typeof(Assessment).GetProperty(crit.AvgProperty)?.SetValue(assessmentEdit, (double)crit.Total);
                }
                else
                {
                    // Se for um critério diferente de Crit1_Clau1 ou Crit8_Clau1, calcular a média
                    typeof(Assessment).GetProperty(crit.AvgProperty)?.SetValue(assessmentEdit, (double)crit.Total / 5);
                }
            }

            // Calcular grand total e overall average
            assessmentEdit.Grand_Tot = criterias.Sum(crit => (double)typeof(Assessment).GetProperty(crit.AvgProperty)?.GetValue(assessmentEdit));
            assessmentEdit.Overall_Average = assessmentEdit.Grand_Tot / 9;

            // Verificar se é apto
            bool apto = assessmentEdit.Grand_Tot >= 70 && assessmentEdit.Overall_Average >= 6.0;

            assessmentEdit.AssessmentResult = apto ? Assessment.APT : Assessment.INAPT;

            // Restante das atualizações
            assessmentEdit.MedicalRestriction = assess.MedicalRestriction;
            assessmentEdit.Crit10_Justification = assess.Crit10_Justification;
            assessmentEdit.ForwardingDate = assess.ForwardingDate;
            assessmentEdit.EvaluatedFor = userType == Convert.ToInt32(UsersTypeEnum.User_Evaluator) ? await _evaluatorServices.EvaluatorsName(idUser) : await _administratorServices.AdministratorsName(idUser);
            assessmentEdit.ModifyDate = assess.ModifyDate;
            assessmentEdit.LastModifiedBy = assess.LastModifiedBy;

            _database.Assessments.Update(assessmentEdit);
            await _database.SaveChangesAsync();

            return assessmentEdit;
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ASSESSMENTS_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ASSESSMENTS_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            assess = null;
            return assess;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ASSESSMENTS_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[ASSESSMENTS_SERVICE]: {Description}", nullException.StackTrace.Trim());

            assess = null;
            return assess;
        }
    }

    public async Task<ICollection<Assessment>> AssessmentsList()
    {
        try
        {
            ICollection<Assessment> test = await _database.Assessments.ToListAsync();

            if (test?.Count == 0)
                throw new ArgumentNullException(nameof(test), ExceptionMessages.ErrorArgumentNullException);

            return test ?? new List<Assessment>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ASSESSMENT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ASSESSMENT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Assessment>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ASSESSMENT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[ASSESSMENT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<Assessment>();
        }
    }

    public async Task<ICollection<Assessment>> AssessmentsList(int userEvaluatorId)
    {
        try
        {
            ICollection<Assessment> assessments = await _database.Assessments.Where(x => x.CivilServant.UserEvaluatorId1 == userEvaluatorId || x.CivilServant.UserEvaluatorId2 == userEvaluatorId).ToListAsync();

            if (assessments?.Count == 0)
                throw new ArgumentNullException(nameof(userEvaluatorId), ExceptionMessages.ErrorArgumentNullException);

            return assessments ?? new List<Assessment>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ASSESSMENT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ASSESSMENT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Assessment>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ASSESSMENT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[ASSESSMENT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<Assessment>();
        }
    }

    public async Task<string> ServantName(int? CivilServantId)
    {
        ICollection<CivilServant> servant = await _database.Servants.Where(x => x.Id == CivilServantId).ToListAsync();
        return servant.FirstOrDefault().Name;
    }

    public int? ServantMasp(int CivilServantId)
    {
        ICollection<CivilServant> servant = _database.Servants.Where(x => x.Id == CivilServantId).ToList();
        return servant.FirstOrDefault().Masp;
    }

    public async Task<bool> IsUnderLicense(int civilServantId)
    {
        CivilServant civilServant = await _database.Servants.FindAsync(civilServantId);
        return civilServant != null && civilServant.UserStats == UserStatsEnum.UnderLicense;
    }

    public Assessment SearchForId(int id)
    {
        return _database.Assessments.FirstOrDefault(x => x.Id == id);
    }
}