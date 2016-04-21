using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLCLightCL.Model;
using System.Data.SqlClient;
using System.Threading;
using PLCLightCL.CusException;
using PLCLightCL.Enum;

namespace PLCLightCL
{
    public class LEPSController
    {
        private string dbConnectString;
        public LEPSController() { }
        
        public LEPSController(string dbConnectString) {
            this.dbConnectString = dbConnectString;
        }
        const string APP_NAME = "ELABEL-BT";
        const string USER_NAME = "ass_user";
        const int TIMEOUT = 10;//10s

        //const int INIT_PROCESS_STATUS=0;
        //const int START_PROCESS_STATUS=60;
        //const int PROCESSING_PROCESS_STATUS = 64;
        //const int COMPLETE_PROCESS_STATUS = 70;

        /// <summary>
        /// get harness by board nr
        /// </summary>
        /// <param name="boardNr"></param>
        /// <returns></returns>
        private LEPSResult StartHarnessByBoard(string boardNr, string workplaceNr)
        {
            string workplace = workplaceNr;
            string nodename = "";
            string ksk = "";
            string kskType = "";
            string anline = "";
            string steering = "";
            string board = boardNr;
            int? processStatus = (int)LEPSProcessStatus.REQUEST_START;
            int? result = 0;
            int? manual_In = 0;
            int? errorCode = 0;
            long? bint_ErrorID = null;

            LEPSResult lresult = (LEPSResult)SetHead(workplace, nodename, APP_NAME, USER_NAME, ksk, kskType, anline, steering, board,
                processStatus, result, manual_In, errorCode, ref bint_ErrorID);
            return lresult;
        }

        /// <summary>
        /// get harness by workplace, if 
        /// </summary>
        /// <param name="workplaceNr"></param>
        /// <returns></returns>
        public HeadMessage GetHarnessByWorkplace(string workplaceNr)
        {
            string nodename = null;
            string ksk = null;
            string kskType = null;
            string anline = null;
            string steering = null;
            string board = null;
            int? processStatus = null;
            int? result = null;
            int? manual_In = null;
            int? errorCode = null;
            long? bint_ErrorID = null;

            GetHead(workplaceNr,nodename, APP_NAME, USER_NAME,
                ref ksk, ref kskType, ref anline, ref steering, ref board, ref processStatus, ref result, ref manual_In, ref errorCode, ref bint_ErrorID);

            HeadMessage msg = new HeadMessage()
            {
                KSK = ksk,
                KSKType = kskType,
                ANLIE = anline,
                Steering = steering,
                Board = board,
                ProcessStatus = processStatus.HasValue ? (LEPSProcessStatus)(processStatus.Value) : LEPSProcessStatus.NULL,
                Result = result,
                ManualIn = manual_In,
                ErrorCode = errorCode,
                BintErrorID = bint_ErrorID
            };
            return msg;
        }
        
        /// <summary>
        /// start harness
        /// </summary>
        /// <param name="boarNr"></param>
        /// <param name="workplaceNr"></param>
        /// <param name="harnessNr"></param>
        /// <returns></returns>
        private int StartHarness(string boardNr, string workplaceNr, string KSK)
        {
            //throw new NotImplementedException();

            string workplace = workplaceNr;
            string nodename = "";
            string ksk = KSK;
            string kskType = "";
            string anline = "";
            string steering = "";
            string board = "";
            int? processStatus = (int) LEPSProcessStatus.REQUEST_START;
            int? result = 0;
            int? manual_In = 0;
            int? errorCode = 0;
            long? bint_ErrorID = null;

            int i = SetHead(workplace, nodename, APP_NAME, USER_NAME, ksk, kskType, anline, steering, board,
                processStatus, result, manual_In, errorCode, ref bint_ErrorID);
            return i; 
        }

        /// <summary>
        /// get & get ksk
        /// </summary>
        /// <param name="boardNr"></param>
        /// <param name="workplaceNr"></param>
        /// <param name="KSK"></param>
        /// <returns></returns>
        public HeadMessage StartAndGetHarnessByBoard(string boardNr, string workplaceNr) {
            //STEP 使用boardnr, 去开始这个工作台的线束,reult=0,processStatus=60
            StartHarnessByBoard(boardNr,workplaceNr);
     
            DateTime startTime = DateTime.Now;
            HeadMessage msg=null;
            while (true)
            {
                msg = GetHarnessByWorkplace(workplaceNr);
                if (msg.KSK != null && msg.KSK.Length > 0)
                {
                    break;
                }
                DateTime endTime = DateTime.Now;
                if ((endTime - startTime).TotalSeconds > TIMEOUT)
                {
                    throw new LEPSTimeOutException();
                }

            }
            if (msg != null)
            {
                if (msg.ProcessStatus == LEPSProcessStatus.PORCESSING
                    && msg.Result.Value == (int)LEPSResult.OK)
                {

                }
                else {
                 //   throw new LEPSStartRequestException();
                }
            }
            return msg;
        }

        /// <summary>
        /// complete basic module
        /// </summary>
        /// <param name="workplaceNr"></param>
        /// <param name="KSK"></param>
        /// <param name="moduleNr"></param>
        /// <returns></returns>
        public LEPSAKMoudleResult AKBasicModule(string productLine, string workplaceNr, string KSK, string moduleNr)
        {
            string ksk = KSK;
            string line = productLine;
            string workplace = workplaceNr;
            string module = moduleNr;
            string scanCode = string.Empty;
            string nodename = string.Empty;
            long? bint_ErrorID = null;
            LEPSAKMoudleResult result = LEPSAKMoudleResult.NIK;
            using (LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext(this.dbConnectString))
            {
                result = (LEPSAKMoudleResult)dc.ackLData_pic(ksk, line, workplace, module, scanCode, nodename, APP_NAME, USER_NAME, ref bint_ErrorID);
            }
            return result;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// complete harness
        /// </summary>
        /// <param name="boardNr"></param>
        /// <param name="workplaceNr"></param>
        /// <param name="harnessNr"></param>
        /// <returns></returns>
        private LEPSResult CompleteHarnessByKSK(string boardNr, string workplaceNr, string KSK)
        {
            string workplace = workplaceNr;
            string nodename = string.Empty;
            string ksk = KSK;
            string kskType = string.Empty;
            string anline = string.Empty;
            string steering = string.Empty;
            string board = boardNr;
            int? processStatus = (int)LEPSProcessStatus.COMPLETE;
            int? result = 1;
            int? manual_In = 0;
            int? errorCode = 0;
            long? bint_ErrorID = null;

           LEPSResult lresult =(LEPSResult)SetHead(workplace, nodename, APP_NAME, USER_NAME, ksk, kskType, anline, steering, board,
                processStatus, result, manual_In, errorCode, ref bint_ErrorID);
           return lresult;
            
           // throw new NotImplementedException();
        }


        public HeadMessage CompleteHarness(string boardNr, string workplaceNr,string KSK)
        {
            //STEP 使用boardnr, 去开始这个工作台的线束,reult=0,processStatus=60
            CompleteHarnessByKSK(boardNr, workplaceNr,KSK);

            DateTime startTime = DateTime.Now;
            HeadMessage msg = null;
            while (true)
            {
                msg = GetHarnessByWorkplace(workplaceNr);
                if (msg.KSK != null && msg.KSK.Length >0 && msg.ProcessStatus==LEPSProcessStatus.INIT) 
                {
                    break;
                }
                DateTime endTime = DateTime.Now;
                if ((endTime - startTime).TotalSeconds > TIMEOUT)
                {
                    throw new LEPSTimeOutException();
                }
            }
            if (msg != null)
            {
                if ( msg.Result.Value == (int)LEPSResult.OK)
                {

                }
                else {
                    throw new LEPSCompleteRequestException();
                }
            }
            return msg;
        }

        /// <summary>
        /// get basic module
        /// </summary>
        /// <param name="boardNr"></param>
        /// <param name="workplaceNr"></param>
        /// <param name="KSK"></param>
        /// <returns></returns>
        public List<string> GetBasicModule(string workplaceNr, string KSK) {
            List<string> basicModules = new List<string>();
            using (LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext(this.dbConnectString))
            {
               //basicModules= dc.LDat_Basic_Modules
               //    .Where(b => b.KSK.Equals(KSK) && b.Pickplace.Equals(workplaceNr))
               //    .Select(b=>b.Basic_Module).Distinct().ToList();
                basicModules = dc.LDat_PIC.Where(
                    m => m.KSK.Equals(KSK) && m.Pickplace.Equals(workplaceNr) && m.Module.StartsWith("WI-"))
                    .Select(m => m.Module).Distinct().ToList();
                    
            }
            return basicModules;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// get Module
        /// </summary>
        /// <param name="workplaceNr"></param>
        /// <param name="KSK"></param>
        /// <returns></returns>
        public List<string> GetModule(string workplaceNr, string KSK) {
            List<string> modules = new List<string>();
            using (LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext(this.dbConnectString)) 
            {
                modules = dc.LDat_PIC.Where(m => m.KSK.Equals(KSK) && m.Pickplace.Equals(workplaceNr))
                    .Select(m => m.Module).Distinct().ToList();
            }
            return modules;
        }

        private int SetHead(
           string workplace, string nodename, string appname, string username,
            string ksk, string kskType, string anline, string steering,  string board,
            int? processStatus, int? result, int? manual_In , int? errorCode ,
         ref   long? bint_ErrorID)
        {
            using (LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext(this.dbConnectString))
            {
              dc.setHeadRecord(ksk, kskType, anline, steering, workplace, board, processStatus, result, manual_In,
                    errorCode, nodename, appname, username,
                    ref bint_ErrorID);
              dc.SubmitChanges();
            }
            return 1;
        }

        private HeadMessage GetHead(
            string workplace, string nodename, string appname, string username,
           ref  string ksk,  ref string kskType,  ref string anline, ref  string steering,  ref string board,
           ref  int? processStatus, ref int? result, ref int? manual_In, ref int? errorCode,
            ref long? bint_ErrorID
            )
        {
            using (LEPSDataClassesDataContext dc = new LEPSDataClassesDataContext(this.dbConnectString))
            {

                int i = dc.getHeadRecord(workplace, nodename, appname, username,
                   ref ksk, ref kskType, ref anline, ref steering, ref board,
                   ref processStatus, ref result, ref manual_In, ref errorCode, ref bint_ErrorID);
            }
            return null;
        }
    }
}
