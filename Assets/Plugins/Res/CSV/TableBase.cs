using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class TableBase{

	public delegate void OnInitHandle(TableBase table);
	public event OnInitHandle OnInit;
	public delegate void OnErrorHandle(TableBase table, string errorContent);
	public event OnErrorHandle OnError;
	private string tablePath;
	protected  List<string[]> tableData;
	/// <summary>
	/// Initializes a new instance of the TableBase class.
	/// </summary>
	/// <param>Table 地址.</param>
	public TableBase(string tablePath){
		this.tablePath = tablePath;
	}
	public void Init(string tablePath = null){
		if(tablePath != null){
			this.tablePath = tablePath;
			resetTable();
		}
		loadTable ();
	}
	public string GetTablePath(){
		return tablePath;
	}
	//加载表格资源
	private void loadTable(){
// #if UNITY_EDITOR
// 		if(Application.isPlaying){
//             BundleHelper.Instance.LoadAsync(GameTable.bundleName, tablePath, (TextAsset asset) =>
//             {
//                 init(asset);
//             });
// 		}
// 		else{
            
            TextAsset textAsset = Resources.Load<TextAsset>(tablePath);
            init (textAsset);
//             TextAsset textAsset = BundleHelper.Instance.Load<TextAsset>(GameTable.bundleName, tablePath);
//             init(textAsset);
// 		}
// #else
//         BundleHelper.Instance.LoadAsync(GameTable.bundleName, tablePath, (TextAsset asset) =>
//         {
//             init(asset);
//         });
// #endif
    }
	
	private void init(TextAsset textAsset){
		string content = textAsset.text;
        content = content.Replace("\\n", "\n");
        string[] rowList = Regex.Split(content, "\r\n");
		tableData = new List<string[]> ();
		for(int i = 0; i < rowList.Length; i++){
			tableData.Add(rowList[i].Split(','));
		}
		//解析
		parseTable ();
		if(OnInit != null)
			OnInit (this);
		OnInit = null;
		OnError = null;
	}
	//重置表格
	virtual protected void resetTable(){
		//LogManager.Error ("Reset Need override!");
	}
	//解析表格
	virtual protected void parseTable(){
		//LogManager.Error ("ParseTable Need override!");
	}
}