using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ScmWcfService.Model;
using ScmWcfService.Model.Message;

namespace ScmWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PickService”。
    public class PickService : IPickService
    {
        public ResponseMessage<Pick> CreatePickByOrderId(int order_id)
        {
            throw new NotImplementedException();
        }

        public ResponseMessage<Pick> GetPickItemsByPickId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
