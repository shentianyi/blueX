using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ScmWcfService.Model.Enum;

namespace ScmWcfService.Model
{
    [DataContract]
    public class OrderBox
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string nr { get; set; }

        [DataMember]
        public string rfid_nr { get; set; }

        [DataMember]
        public OrderBoxStatus status { get; set; }

        [DataMember]
        public OrderBoxType order_box_type { get; set; }

        [DataMember]
        public Warehouse warehouse { get; set; }


        [DataMember]
        public Position position { get; set; }

        [DataMember]
        public Warehouse source_warehouse { get; set; }

        [DataMember]
        public Part part { get; set; }

        [DataMember]
        public float? quantity { get; set; }

        [DataMember]
        public float? stock { get; set; }

        [DataMember]
        public List<string> positions { get; set; }


        // partial for view
        public string box_type_name { get { return  this.order_box_type==null ? "" : this.order_box_type.name; } }
        public string warehouse_nr { get { return this.warehouse == null ? "" : this.warehouse.nr; } }
        public string position_nr { get { return this.position == null ? "" : this.position.nr; } }
        public string part_nr { get { return this.part == null ? "" : this.part.nr; } }
        public string source_warehouse_nr { get { return this.source_warehouse == null ? "" : this.source_warehouse.nr; } }
        public bool over_stock { get { return this.quantity > stock; } }
        public string positions_nr { get { return this.positions == null ? "" : string.Join(",", this.positions); } }
        public string status_display { get { return OrderBoxStatusDisplay.GetStatusOfOrderBox(this.status); } }
    }
}