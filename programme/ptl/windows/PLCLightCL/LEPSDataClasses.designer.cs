﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PLCLightCL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="LEPS")]
	public partial class LEPSDataClassesDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertLDat_PIC(LDat_PIC instance);
    partial void UpdateLDat_PIC(LDat_PIC instance);
    partial void DeleteLDat_PIC(LDat_PIC instance);
    #endregion
		
		public LEPSDataClassesDataContext() : 
				base(global::PLCLightCL.Properties.Settings.Default.LEPSConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public LEPSDataClassesDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LEPSDataClassesDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LEPSDataClassesDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public LEPSDataClassesDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<LDat_PIC> LDat_PIC
		{
			get
			{
				return this.GetTable<LDat_PIC>();
			}
		}
		
		public System.Data.Linq.Table<LDat_Basic_Modules> LDat_Basic_Modules
		{
			get
			{
				return this.GetTable<LDat_Basic_Modules>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="COM_ASS.ackLData_pic")]
		public int ackLData_pic([global::System.Data.Linq.Mapping.ParameterAttribute(Name="KSK", DbType="VarChar(20)")] string kSK, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Line", DbType="VarChar(10)")] string line, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="WorkPlace", DbType="VarChar(10)")] string workPlace, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Module", DbType="VarChar(100)")] string module, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ScanCode", DbType="VarChar(100)")] string scanCode, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_NodeName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_ApplicationName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_UserName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] ref System.Nullable<long> bint_ErrorID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), kSK, line, workPlace, module, scanCode, str_NodeName, str_ApplicationName, str_UserName, bint_ErrorID);
			bint_ErrorID = ((System.Nullable<long>)(result.GetParameterValue(8)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="COM_ASS.getHeadRecord")]
		public int getHeadRecord([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Workplace", DbType="VarChar(10)")] string workplace, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(100)")] string str_NodeName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(100)")] string str_ApplicationName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(100)")] string str_UserName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="KSK", DbType="VarChar(20)")] ref string kSK, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="KSKType", DbType="VarChar(10)")] ref string kSKType, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ANLIE", DbType="VarChar(20)")] ref string aNLIE, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Steering", DbType="VarChar(2)")] ref string steering, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Board", DbType="VarChar(5)")] ref string board, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProcessStatus", DbType="Int")] ref System.Nullable<int> processStatus, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Result", DbType="Int")] ref System.Nullable<int> result, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Manual_In", DbType="Int")] ref System.Nullable<int> manual_In, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorCode", DbType="Int")] ref System.Nullable<int> errorCode, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] ref System.Nullable<long> bint_ErrorID)
		{
			IExecuteResult result1 = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), workplace, str_NodeName, str_ApplicationName, str_UserName, kSK, kSKType, aNLIE, steering, board, processStatus, result, manual_In, errorCode, bint_ErrorID);
			kSK = ((string)(result1.GetParameterValue(4)));
			kSKType = ((string)(result1.GetParameterValue(5)));
			aNLIE = ((string)(result1.GetParameterValue(6)));
			steering = ((string)(result1.GetParameterValue(7)));
			board = ((string)(result1.GetParameterValue(8)));
			processStatus = ((System.Nullable<int>)(result1.GetParameterValue(9)));
			result = ((System.Nullable<int>)(result1.GetParameterValue(10)));
			manual_In = ((System.Nullable<int>)(result1.GetParameterValue(11)));
			errorCode = ((System.Nullable<int>)(result1.GetParameterValue(12)));
			bint_ErrorID = ((System.Nullable<long>)(result1.GetParameterValue(13)));
			return ((int)(result1.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="COM_ASS.setHeadRecord")]
		public int setHeadRecord([global::System.Data.Linq.Mapping.ParameterAttribute(Name="KSK", DbType="VarChar(20)")] string kSK, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="KSKType", DbType="VarChar(10)")] string kSKType, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ANLIE", DbType="VarChar(20)")] string aNLIE, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Steering", DbType="VarChar(2)")] string steering, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="WorkPlace", DbType="VarChar(10)")] string workPlace, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Board", DbType="VarChar(100)")] string board, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProcessStatus", DbType="Int")] System.Nullable<int> processStatus, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Result", DbType="Int")] System.Nullable<int> result, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Manual_In", DbType="Int")] System.Nullable<int> manual_In, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorCode", DbType="Int")] System.Nullable<int> errorCode, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_NodeName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_ApplicationName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(30)")] string str_UserName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="BigInt")] ref System.Nullable<long> bint_ErrorID)
		{
			IExecuteResult result1 = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), kSK, kSKType, aNLIE, steering, workPlace, board, processStatus, result, manual_In, errorCode, str_NodeName, str_ApplicationName, str_UserName, bint_ErrorID);
			bint_ErrorID = ((System.Nullable<long>)(result1.GetParameterValue(13)));
			return ((int)(result1.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="COM_ASS.LDat_PIC")]
	public partial class LDat_PIC : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Line;
		
		private string _KSK;
		
		private string _KSKType;
		
		private string _WorkPlace;
		
		private string _Pickplace;
		
		private string _Module;
		
		private string _ScanMask;
		
		private int _ScanCodeMin;
		
		private int _ScanCodeMax;
		
		private System.Nullable<int> _Shelf;
		
		private System.Nullable<int> _Rank;
		
		private System.Nullable<int> _ItemPlace;
		
		private System.Nullable<int> _PickOrder;
		
		private System.Nullable<bool> _Released;
		
		private System.Nullable<System.DateTime> _PickTime;
		
		private string _Status;
		
		private string _ScanCode;
		
		private System.DateTime _CreateTime;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLineChanging(string value);
    partial void OnLineChanged();
    partial void OnKSKChanging(string value);
    partial void OnKSKChanged();
    partial void OnKSKTypeChanging(string value);
    partial void OnKSKTypeChanged();
    partial void OnWorkPlaceChanging(string value);
    partial void OnWorkPlaceChanged();
    partial void OnPickplaceChanging(string value);
    partial void OnPickplaceChanged();
    partial void OnModuleChanging(string value);
    partial void OnModuleChanged();
    partial void OnScanMaskChanging(string value);
    partial void OnScanMaskChanged();
    partial void OnScanCodeMinChanging(int value);
    partial void OnScanCodeMinChanged();
    partial void OnScanCodeMaxChanging(int value);
    partial void OnScanCodeMaxChanged();
    partial void OnShelfChanging(System.Nullable<int> value);
    partial void OnShelfChanged();
    partial void OnRankChanging(System.Nullable<int> value);
    partial void OnRankChanged();
    partial void OnItemPlaceChanging(System.Nullable<int> value);
    partial void OnItemPlaceChanged();
    partial void OnPickOrderChanging(System.Nullable<int> value);
    partial void OnPickOrderChanged();
    partial void OnReleasedChanging(System.Nullable<bool> value);
    partial void OnReleasedChanged();
    partial void OnPickTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnPickTimeChanged();
    partial void OnStatusChanging(string value);
    partial void OnStatusChanged();
    partial void OnScanCodeChanging(string value);
    partial void OnScanCodeChanged();
    partial void OnCreateTimeChanging(System.DateTime value);
    partial void OnCreateTimeChanged();
    #endregion
		
		public LDat_PIC()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Line", DbType="VarChar(10) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Line
		{
			get
			{
				return this._Line;
			}
			set
			{
				if ((this._Line != value))
				{
					this.OnLineChanging(value);
					this.SendPropertyChanging();
					this._Line = value;
					this.SendPropertyChanged("Line");
					this.OnLineChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_KSK", DbType="VarChar(20) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string KSK
		{
			get
			{
				return this._KSK;
			}
			set
			{
				if ((this._KSK != value))
				{
					this.OnKSKChanging(value);
					this.SendPropertyChanging();
					this._KSK = value;
					this.SendPropertyChanged("KSK");
					this.OnKSKChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_KSKType", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string KSKType
		{
			get
			{
				return this._KSKType;
			}
			set
			{
				if ((this._KSKType != value))
				{
					this.OnKSKTypeChanging(value);
					this.SendPropertyChanging();
					this._KSKType = value;
					this.SendPropertyChanged("KSKType");
					this.OnKSKTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WorkPlace", DbType="VarChar(10) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string WorkPlace
		{
			get
			{
				return this._WorkPlace;
			}
			set
			{
				if ((this._WorkPlace != value))
				{
					this.OnWorkPlaceChanging(value);
					this.SendPropertyChanging();
					this._WorkPlace = value;
					this.SendPropertyChanged("WorkPlace");
					this.OnWorkPlaceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pickplace", DbType="VarChar(10) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Pickplace
		{
			get
			{
				return this._Pickplace;
			}
			set
			{
				if ((this._Pickplace != value))
				{
					this.OnPickplaceChanging(value);
					this.SendPropertyChanging();
					this._Pickplace = value;
					this.SendPropertyChanged("Pickplace");
					this.OnPickplaceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Module", DbType="VarChar(100) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Module
		{
			get
			{
				return this._Module;
			}
			set
			{
				if ((this._Module != value))
				{
					this.OnModuleChanging(value);
					this.SendPropertyChanging();
					this._Module = value;
					this.SendPropertyChanged("Module");
					this.OnModuleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScanMask", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string ScanMask
		{
			get
			{
				return this._ScanMask;
			}
			set
			{
				if ((this._ScanMask != value))
				{
					this.OnScanMaskChanging(value);
					this.SendPropertyChanging();
					this._ScanMask = value;
					this.SendPropertyChanged("ScanMask");
					this.OnScanMaskChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScanCodeMin", DbType="Int NOT NULL")]
		public int ScanCodeMin
		{
			get
			{
				return this._ScanCodeMin;
			}
			set
			{
				if ((this._ScanCodeMin != value))
				{
					this.OnScanCodeMinChanging(value);
					this.SendPropertyChanging();
					this._ScanCodeMin = value;
					this.SendPropertyChanged("ScanCodeMin");
					this.OnScanCodeMinChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScanCodeMax", DbType="Int NOT NULL")]
		public int ScanCodeMax
		{
			get
			{
				return this._ScanCodeMax;
			}
			set
			{
				if ((this._ScanCodeMax != value))
				{
					this.OnScanCodeMaxChanging(value);
					this.SendPropertyChanging();
					this._ScanCodeMax = value;
					this.SendPropertyChanged("ScanCodeMax");
					this.OnScanCodeMaxChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Shelf", DbType="Int")]
		public System.Nullable<int> Shelf
		{
			get
			{
				return this._Shelf;
			}
			set
			{
				if ((this._Shelf != value))
				{
					this.OnShelfChanging(value);
					this.SendPropertyChanging();
					this._Shelf = value;
					this.SendPropertyChanged("Shelf");
					this.OnShelfChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rank", DbType="Int")]
		public System.Nullable<int> Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				if ((this._Rank != value))
				{
					this.OnRankChanging(value);
					this.SendPropertyChanging();
					this._Rank = value;
					this.SendPropertyChanged("Rank");
					this.OnRankChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ItemPlace", DbType="Int")]
		public System.Nullable<int> ItemPlace
		{
			get
			{
				return this._ItemPlace;
			}
			set
			{
				if ((this._ItemPlace != value))
				{
					this.OnItemPlaceChanging(value);
					this.SendPropertyChanging();
					this._ItemPlace = value;
					this.SendPropertyChanged("ItemPlace");
					this.OnItemPlaceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PickOrder", DbType="Int")]
		public System.Nullable<int> PickOrder
		{
			get
			{
				return this._PickOrder;
			}
			set
			{
				if ((this._PickOrder != value))
				{
					this.OnPickOrderChanging(value);
					this.SendPropertyChanging();
					this._PickOrder = value;
					this.SendPropertyChanged("PickOrder");
					this.OnPickOrderChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Released", DbType="Bit")]
		public System.Nullable<bool> Released
		{
			get
			{
				return this._Released;
			}
			set
			{
				if ((this._Released != value))
				{
					this.OnReleasedChanging(value);
					this.SendPropertyChanging();
					this._Released = value;
					this.SendPropertyChanged("Released");
					this.OnReleasedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PickTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> PickTime
		{
			get
			{
				return this._PickTime;
			}
			set
			{
				if ((this._PickTime != value))
				{
					this.OnPickTimeChanging(value);
					this.SendPropertyChanging();
					this._PickTime = value;
					this.SendPropertyChanged("PickTime");
					this.OnPickTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="VarChar(2) NOT NULL", CanBeNull=false)]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScanCode", DbType="VarChar(100)")]
		public string ScanCode
		{
			get
			{
				return this._ScanCode;
			}
			set
			{
				if ((this._ScanCode != value))
				{
					this.OnScanCodeChanging(value);
					this.SendPropertyChanging();
					this._ScanCode = value;
					this.SendPropertyChanged("ScanCode");
					this.OnScanCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateTime", DbType="DateTime NOT NULL")]
		public System.DateTime CreateTime
		{
			get
			{
				return this._CreateTime;
			}
			set
			{
				if ((this._CreateTime != value))
				{
					this.OnCreateTimeChanging(value);
					this.SendPropertyChanging();
					this._CreateTime = value;
					this.SendPropertyChanged("CreateTime");
					this.OnCreateTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="COM_ASS.LDat_Basic_Modules")]
	public partial class LDat_Basic_Modules
	{
		
		private string _KSK;
		
		private string _Workplace;
		
		private string _Pickplace;
		
		private string _Basic_Module;
		
		private string _Module;
		
		private System.DateTime _CreateTime;
		
		private System.Nullable<System.DateTime> _ProcessTime;
		
		private System.Nullable<int> _ErrorCode;
		
		public LDat_Basic_Modules()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_KSK", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string KSK
		{
			get
			{
				return this._KSK;
			}
			set
			{
				if ((this._KSK != value))
				{
					this._KSK = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Workplace", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Workplace
		{
			get
			{
				return this._Workplace;
			}
			set
			{
				if ((this._Workplace != value))
				{
					this._Workplace = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pickplace", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string Pickplace
		{
			get
			{
				return this._Pickplace;
			}
			set
			{
				if ((this._Pickplace != value))
				{
					this._Pickplace = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Basic_Module", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Basic_Module
		{
			get
			{
				return this._Basic_Module;
			}
			set
			{
				if ((this._Basic_Module != value))
				{
					this._Basic_Module = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Module", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Module
		{
			get
			{
				return this._Module;
			}
			set
			{
				if ((this._Module != value))
				{
					this._Module = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateTime", DbType="DateTime NOT NULL")]
		public System.DateTime CreateTime
		{
			get
			{
				return this._CreateTime;
			}
			set
			{
				if ((this._CreateTime != value))
				{
					this._CreateTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProcessTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> ProcessTime
		{
			get
			{
				return this._ProcessTime;
			}
			set
			{
				if ((this._ProcessTime != value))
				{
					this._ProcessTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorCode", DbType="Int")]
		public System.Nullable<int> ErrorCode
		{
			get
			{
				return this._ErrorCode;
			}
			set
			{
				if ((this._ErrorCode != value))
				{
					this._ErrorCode = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
