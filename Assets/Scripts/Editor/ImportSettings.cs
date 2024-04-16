using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImportSettings : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        var importer = (ModelImporter) assetImporter;
		
        importer.useFileScale = false;
        importer.bakeAxisConversion = true;
    }
}
