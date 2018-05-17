using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class CarTable : TableBase {
	private Dictionary<int, CarElement> ElementList;
	private List<CarElement> Elements = null;
	public CarTable(string tablePath):base(tablePath){
		resetTable ();
	}
	//重置表格
	override protected void resetTable () {
		ElementList = new Dictionary<int, CarElement> ();
		Elements = null;
	}
	//解析表格
	override protected void parseTable () {
        for (int i = 2; i < tableData.Count; i++) {
            string[] row = tableData[i];
            ElementList[int.Parse(row[0])] = new CarElement(row);
        }
	}
	public bool HaveElement(int id){
		return ElementList.ContainsKey (id);
	}
	public CarElement GetElement(int id){
		if(ElementList.ContainsKey(id)){
			return ElementList[id];
		}
		//LogManager.Alert("Car表格中找不到主key为 {0} 的Element", id );
        return null;
	}

	public List<CarElement> GetAllElement(){
		if (Elements == null) {
			Elements = new List<CarElement>();
			foreach(int key in ElementList.Keys){
				Elements.Add(ElementList[key]);
			}
		}
		return Elements;
	}
}


public class CarElement {
	private Dictionary<int, object> m_valueList = new Dictionary<int, object> ();

	///汽车序号
	public int ID;
	///汽车名称
	public string Name;
	///图片名称
	public string SpriteName;
	///后门
	public int Postern;
	///内在表现
	public string[] InternalPerformance;
	///外表感觉
	public string[] Appearance;

    public CarElement(string[] elementData) {
		this.ID = int.Parse(elementData[0]);
		this.Name = elementData[1];
		this.SpriteName = elementData[2];
		this.Postern = int.Parse(elementData[3]);
		if(elementData[4] != ""){
			this.InternalPerformance = elementData[4].Split('|');
		}else{
			this.InternalPerformance = new string[0];
		}

		if(elementData[5] != ""){
			this.Appearance = elementData[5].Split('|');
		}else{
			this.Appearance = new string[0];
		}


    }

	public object this[string valueName]{
		get{
			int hashCodeKey = valueName.GetHashCode ();
			if (m_valueList.ContainsKey (hashCodeKey)) {
				return m_valueList [hashCodeKey];
			} else {
                //LogManager.Alert("Car表格中找不到名字为{0}的变量", valueName);
				return null;
			}
		}
	}
}