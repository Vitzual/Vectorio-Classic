//////////////////////////////////////////////////////
// MK Glow IDimension	 	    	    	       	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
namespace MK.Glow
{
	/// <summary>
	/// Representation of the size of a render context
	/// </summary>
	internal interface IDimension
	{
		int width { get; }
		int height { get; }
		RenderDimension renderDimension { get; }
	}
}
