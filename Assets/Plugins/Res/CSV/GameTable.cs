using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
#if UNITY_EDITOR
using System.Reflection;
#endif

public class GameTable{

	public delegate void OnInitCompleteHandle();
	static public event OnInitCompleteHandle OnInitComplete;//所有表格初始化完成调用
	public delegate void OnInitTableHandle(string tableName, int currentNum, int totalNum);//一张表格初始化完成
	static public event OnInitTableHandle OnInitTable;//所有表格初始化完成调用
	public const string bundleName = "config";

	static private List<TableBase> tableList;//所有表格的序列
	static private Dictionary<Type, TableBase> tableDic;//所有表格的字典
	static int flag = 0;//记录当前初始化第几个表
    static bool isInit = false;
	///开始初始化
	static public void Init(string publicPath = ""){
        if (isInit)
            return;
        isInit = true;
		tableList = new List<TableBase> ();
		tableList.Add(new CarTable(publicPath + "Car"));

		initNextTable ();
	}
	//强制重新初始化
	static public void ForceInit(string publicPath = "")
	{
		isInit = false;
		flag = 0;
		Init(publicPath);
	}
	///初始化下一个表格
	static private void initNextTable(){
		if (flag >= tableList.Count) {
			tableDic = new Dictionary<Type, TableBase> ();
			for(int i = 0; i < tableList.Count; i ++){
				tableDic.Add (tableList[i].GetType(), tableList[i]);
			}
			tableList = null;
			if (OnInitComplete != null) {
				OnInitComplete ();
			}
		} 
		else {
			tableList[flag].OnInit += onTableInit;
			tableList[flag].OnError += onTableError;
			tableList[flag].Init();
		}
	}
	///一个表格初始化完成
	static private void onTableInit(TableBase table){
		table.OnInit -= onTableInit;
		flag++;
		if (OnInitTable != null) {
			OnInitTable(table.GetType().ToString(), flag, tableList.Count);
		}
		initNextTable ();
	}
	///一个表格初始化错误
	static private void onTableError(TableBase table, string errorContent){
		table.OnError -= onTableError;
        //LogManager.Error("表格初始化错误：table={0} error={1}" , table.GetType().ToString() , errorContent);
	}
	///获取一个类型指定的模块
	static public T GetTable<T>() where T : TableBase{
		return tableDic[typeof(T)] as T;
	}
	#if UNITY_EDITOR
	/// <summary>将一个表格的Element转化为String</summary>
	/// <returns>转化后的字符串</returns>
	/// <param name="obj">Element对象</param>
	/// <param name="separator">值分隔符</param>
	/// <param name="arrSeparator">数组元素分隔符</param>
	static public string ElementToString(object obj, string separator = ",", string arrSeparator = "|", string zeroFlag = ""){
		Type type = obj.GetType();   
		System.Reflection.FieldInfo[] fieldInfos = type.GetFields();
		StringBuilder sb = new StringBuilder();
		foreach (FieldInfo fi in fieldInfos) {
			if (sb.Length > 0)
				sb.Append (separator);
			string name = fi.Name;
			object value = fi.GetValue (obj);
			if (fi.FieldType == typeof(int[])) {
				int[] intArr = (int[])value;
				string str = "";
				for(int i = 0; i < intArr.Length; i++){
					if(string.IsNullOrEmpty(str) == false)
						str += arrSeparator;
					str += intArr [i];
				}
				sb.Append (str);
			} else if(fi.FieldType == typeof(string[])){
				string[] stringArr = (string[])value;
				string str = "";
				for(int i = 0; i < stringArr.Length; i++){
					if(string.IsNullOrEmpty(str) == false)
						str += arrSeparator;
					str += stringArr [i];
				}
				sb.Append (str);
			} else{
				if (fi.FieldType == typeof(int) && (int)value == 0) {
					sb.Append (zeroFlag);
				} else {
					sb.Append (value);
				}
			}
		}
		return sb.ToString();
	}
	#endif
}
