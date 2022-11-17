using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MasterData : ScriptableObject
{
	public List<MaterialEntity> MaterialMaster;
	public List<ToolEntity> ToolMaster;
	public List<EquipmentEntity> EquipmentMaster;
}
