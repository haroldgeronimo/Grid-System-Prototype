using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
	public Material[] occupied;
	public Material[] free;
	public Camera camera = new Camera();
	public gridManager grid;
	public GameObject showObj;
	public Objects selectedObject;
	public LayerMask ignoreObjects;
	//temporary objects
		GameObject objTrans; //gameobject that will track the rotation of placement
		List<GameObject> placeholders = new List<GameObject>();

	private void Start() {
	objTrans = new GameObject();
	}
	// Update is called once per frame
	void LateUpdate () {
		RaycastHit hit; 
		Cell c = null;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
		int layerMask = 9;

		if ( Physics.Raycast (ray,out hit,Mathf.Infinity,layerMask)) {
			c =  hit.transform.gameObject.GetComponent<Cell>();

			if(c == null){
				//Debug.Log("null")
				return;
			}
			
				deletePlaceholders();
				if(ShowObject(c,selectedObject)){
					if(Input.GetMouseButtonDown(0)){
							PlaceObject(c,selectedObject);
					}
				}
			
		}
		else{
			deletePlaceholders();
		}

	
		
		if(Input.GetMouseButtonDown(1)){
			Debug.Log("Delete");
				if ( Physics.Raycast (ray,out hit,Mathf.Infinity)) {
				//check to parents
				Objects obj =  hit.transform.gameObject.GetComponent<Objects>();
				if(obj != null){
					
					Debug.Log("Deleting");
					DeleteObject(hit.transform);
					}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.E)){
			selectedObject = RotateObject(true,selectedObject);
		}

		if(Input.GetKeyDown(KeyCode.Q)){
		selectedObject=	RotateObject(false,selectedObject);
		}


	}


	//TRANSFERABLE TO CELL OR GRID CLASS?
	void ToggleObject(Cell c) {

		 if(c.isOccupied){
			c.isOccupied = false;
		 }else{
			c.isOccupied = true;
		 }


 	}
	
	//TRANSFERABLE TO OBJECTS CLASS?
	 Objects RotateObject(bool isLeft, Objects obj){
		
		 //rotate data
		 foreach(Unit unit in obj.dimension.units){
			 if(isLeft){
			 unit.row *= -1;
		 Debug.Log(obj.prefab.transform.rotation);
			 }
			 else{
			 unit.col *= -1;

			 }

			 unit.row = unit.row + unit.col;
			 unit.col = unit.row - unit.col;
			 unit.row = unit.row - unit.col;
		 }
		 
		 //rotate object
		 if(isLeft)
		 objTrans.transform.Rotate(0,90,0);
		 else
		 objTrans.transform.Rotate(0,-90,0);
		 return obj;
	 }

//TRANSFERABLE TO OBJECTS OR GRID CLASS?
	 void PlaceObject(Cell c,Objects obj ){
		/*
		 float x = (grid.startPt.x + ((c.row+1)*grid.cellSize.x)) + (grid.offset.x*c.row);
		 float y = grid.startPt.y + grid.offset.y + grid.cellSize.y + (showObj.transform.localScale.y /2);
		 float z = (grid.startPt.z + ((c.col+1)*grid.cellSize.z)) + (grid.offset.z*c.col);

		// -gridLength/2 + 0.5f +i,0,-gridWidth/2 + 0.5f + j
		
	

		GameObject go  = Instantiate(selectedObject.prefab,new Vector3(x,y,z),Quaternion.identity);
		 go.transform.rotation = objTrans.transform.rotation;
		Objects newObj = go.AddComponent<Objects>();
		newObj.dimension =  obj.dimension;
		newObj.title =  obj.title;
		newObj.centerRow = c.row;
		newObj.centerCol = c.col;
		grid.gridObjects.Add(newObj);

		foreach(Unit unit in obj.dimension.units)
		 {
			 
		 ToggleObject(grid.gridCells[c.row + unit.row,c.col + unit.col]);
		 }
 */
		 
	 }

	 void DeleteObject(Transform trans){
/* 
		 Objects obj = trans.gameObject.GetComponent<Objects>();
		 //clear occupation from grid
		 foreach (Unit unit in obj.dimension.units)
			 grid.gridCells[unit.row + obj.centerRow,unit.col + obj.centerCol].isOccupied = false;

		 //delete record from grid
		grid.gridObjects.Remove(obj);
		 //destroy object
		 Destroy(trans.gameObject);

		 Debug.Log("deleted");
		*/ 
	 }

	 bool ShowObject(Cell c, Objects obj){


        int h = GetHeight(c);
//display placeholders

		 float x,y,z;
		 placeholders = new List<GameObject>();
		 bool isPlaceable = true;
		 
		 for (int i = h; i < obj.dimension.height + h; i++)
		 {
			foreach(Unit unit in obj.dimension.units)
			{
				//position
				/* 
				x = (grid.startPt.x + ((c.row+ unit.row+1)*grid.cellSize.x)) + (grid.offset.x*(c.row+ unit.row));
				y = grid.startPt.y + grid.offset.y + (showObj.transform.localScale.y /2) ;
				z = (grid.startPt.z + ((c.col+ unit.col+1)*grid.cellSize.z)) + (grid.offset.z*(c.col+ unit.col));
				/ -gridLength/2 + 0.5f +i,0,-gridWidth/2 + 0.5f + j
				*/
				x = -grid.gridLength/2 + 0.5f + c.row + unit.row;
				y = i + grid.cellSize.y / 2;
				z = -grid.gridWidth/2 + 0.5f + c.col + unit.col;

				//
				GameObject placeholder = Instantiate(showObj,new Vector3(x,y,z),Quaternion.identity);	
			
				if(CheckOccupied(c.row + unit.row,c.col + unit.col, i)){
			
					placeholder.GetComponent<MeshRenderer>().materials = occupied;
					isPlaceable = false;
					if(obj.isOverlayable){
						placeholder.GetComponent<MeshRenderer>().materials = free;
						isPlaceable = true;
					}

				}
				else{
				placeholder.GetComponent<MeshRenderer>().materials = free;	
				}
			
			placeholders.Add(placeholder);
			}
			 
		 }
		 //GameObject placeholder = Instantiate(obj.prefab,new Vector3(x,y,z),Quaternion.identity);
		 
				 x = -grid.gridLength/2 + 0.5f + c.row;
				 y = grid.cellSize.y / 2 + h;
				z = -grid.gridWidth/2 + 0.5f + c.col;
		 
		 
		 GameObject objPlaceholder = Instantiate(obj.prefab,new Vector3(x,y,z),Quaternion.identity);	

		

			if(isPlaceable)
				objPlaceholder.GetComponent<MeshRenderer>().materials = free;
			else
				objPlaceholder.GetComponent<MeshRenderer>().materials = occupied;
			
		objPlaceholder.transform.rotation = objTrans.transform.rotation;
		placeholders.Add(objPlaceholder);
		return isPlaceable;
	 }

//grid methods 

	 void deletePlaceholders(){
		 foreach(GameObject placeholder in placeholders)
		 Destroy(placeholder);
	 }


    /// <summary>
    /// Checks if a cell is occupied. Returns true if occupied or invalid, and false if not occupied.
    /// </summary>
    /// <param name="row">Cell row value</param>
    /// <param name="col">Cell column value</param>
    /// <param name="h">Cell height value</param>
    /// <returns></returns>
    bool CheckOccupied(int row,int col, int h){
    //Can be moved to grid manager
				try
				{
					if(grid.gridCells[row,col,h].isOccupied){//occupied
						return true;
					}
					else{//not occupied
						if(h>0){//check under
								if(grid.gridCells[row,col,h-1].isOccupied)//if occupied
								return false;
								else//if not occupied
								return true;

						}
						else{//if nothing under
							
							return false;	
						}
					}
				}
				catch (System.Exception)
				{
					
					return true;
			
				}
	

	}
    /// <summary>
    /// Get valid Cell height for an obect from a center point 
    /// </summary>
    /// <param name="c">Cell to get valid height</param>
    /// <returns></returns>
    int GetHeight(Cell c)
    {
        //decide height 
        int h = 0;
        for (int i = 0; i < grid.gridHeight; i++)
        {

            Debug.Log("i:" + i);
            if (!CheckOccupied(c.row, c.col, i))
            {
                h = i;
                Debug.Log("BREAAAAAAKKKK!!!");
                break;
            }
        }
        return h;
    }
	
}


/*
		 float x = (grid.startPt.x + ((c.row+1)*grid.cellSize.x)) + (grid.offset.x*c.row);
		 float y = grid.startPt.y + grid.offset.y + grid.cellSize.y + (showObj.transform.localScale.y /2);
		 float z = (grid.startPt.z + ((c.col+1)*grid.cellSize.z)) + (grid.offset.z*c.col);
		 
 */