using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models;

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

    public async Task<Assessment> RegisterAssessments(Assessment assessment)
    {
        try
        {
            if (assessment == null) throw new ArgumentNullException(nameof(assessment), ExceptionMessages.ErrorArgumentNullException);

            await _database.Assessments.AddAsync(assessment);
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

            List<string> propertiesToUpdate = new ()
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
                    var value = assessProperty.GetValue(assess);
                    typeof(Assessment).GetProperty(propertyName)?.SetValue(assessmentEdit, value);
                }
            }

            assessmentEdit.Tot_Crit1 = assess.Crit1_Clau1;
            assessmentEdit.Tot_Crit2 = assess.Crit2_Clau1 + assess.Crit2_Clau2 + assess.Crit2_Clau3 + assess.Crit2_Clau4 + assess.Crit2_Clau5;
            assessmentEdit.Tot_Crit3 = assess.Crit3_Clau1 + assess.Crit3_Clau2 + assess.Crit3_Clau3 + assess.Crit3_Clau4 + assess.Crit3_Clau5;
            assessmentEdit.Tot_Crit4 = assess.Crit4_Clau1 + assess.Crit4_Clau2 + assess.Crit4_Clau3 + assess.Crit4_Clau4 + assess.Crit4_Clau5;
            assessmentEdit.Tot_Crit5 = assess.Crit5_Clau1 + assess.Crit5_Clau2 + assess.Crit5_Clau3 + assess.Crit5_Clau4 + assess.Crit5_Clau5;
            assessmentEdit.Tot_Crit6 = assess.Crit6_Clau1 + assess.Crit6_Clau2 + assess.Crit6_Clau3 + assess.Crit6_Clau4 + assess.Crit6_Clau5;
            assessmentEdit.Tot_Crit7 = assess.Crit7_Clau1 + assess.Crit7_Clau2 + assess.Crit7_Clau3 + assess.Crit7_Clau4 + assess.Crit7_Clau5;
            assessmentEdit.Tot_Crit8 = assess.Crit8_Clau1;
            assessmentEdit.Tot_Crit9 = assess.Crit9_Clau1 + assess.Crit9_Clau2 + assess.Crit9_Clau3 + assess.Crit9_Clau4 + assess.Crit9_Clau5;

            assessmentEdit.Average_Crit1 = Convert.ToDouble(assessmentEdit.Tot_Crit1);
            assessmentEdit.Average_Crit2 = Convert.ToDouble(assessmentEdit.Tot_Crit2) / 5;
            assessmentEdit.Average_Crit3 = Convert.ToDouble(assessmentEdit.Tot_Crit3) / 5;
            assessmentEdit.Average_Crit4 = Convert.ToDouble(assessmentEdit.Tot_Crit4) / 5;
            assessmentEdit.Average_Crit5 = Convert.ToDouble(assessmentEdit.Tot_Crit5) / 5;
            assessmentEdit.Average_Crit6 = Convert.ToDouble(assessmentEdit.Tot_Crit6) / 5;
            assessmentEdit.Average_Crit7 = Convert.ToDouble(assessmentEdit.Tot_Crit7) / 5;
            assessmentEdit.Average_Crit8 = Convert.ToDouble(assessmentEdit.Tot_Crit8);
            assessmentEdit.Average_Crit9 = Convert.ToDouble(assessmentEdit.Tot_Crit9) / 5;

            assessmentEdit.Grand_Tot = assessmentEdit.Average_Crit1 + assessmentEdit.Average_Crit2 + assessmentEdit.Average_Crit3 + assessmentEdit.Average_Crit4 + assessmentEdit.Average_Crit5 + assessmentEdit.Average_Crit6 + assessmentEdit.Average_Crit7 + assessmentEdit.Average_Crit8 + assessmentEdit.Average_Crit9;

            assessmentEdit.Overall_Average = assessmentEdit.Grand_Tot / 9;

            bool apto = assessmentEdit.Average_Crit1 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit2 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit3 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit4 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit5 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit6 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit7 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit8 / 10 * 100 >= 60 &&
            assessmentEdit.Average_Crit9 / 10 * 100 >= 60 &&
            assessmentEdit.Overall_Average >= 70;

            assessmentEdit.AssessmentResult = apto ? Assessment.APT : Assessment.INAPT;

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