using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using System.Runtime.InteropServices;

public class SplineCubique
{
    private float[] Coeffs { get; set; }

    public SplineCubique(List<Vector3>pointsSpline)
    {
        int nbPts = pointsSpline.Count;
        float[] x = new float[nbPts];
        float[] y = new float[nbPts];

        for (int i = 0; i < nbPts; i++)
        {
            x[i] = pointsSpline[i].x;
            y[i] = pointsSpline[i].y;
        }

        float[] deltasX = new float[nbPts - 1];
        float[] alpha = new float[nbPts - 1];

        for (int i = 0; i < nbPts - 1; i++)
        {
            deltasX[i] = x[i + 1] - x[i];
            alpha[i] = 3 / deltasX[i] * (y[i + 1] - y[i]) -
                  (3 / deltasX[Mathf.Max(0, i - 1)]) 
                  * (y[i] - y[Mathf.Max(0, i - 1)]);
        }

        float[] matTriCoeffs1 = new float[nbPts];
        float[] matTriCoeffs2 = new float[nbPts-1];
        float[] matTriCoeffs3 = new float[nbPts];

        matTriCoeffs1[0] = 1;
        matTriCoeffs2[0] = 0;
        matTriCoeffs3[0] = 0;

        for (int i = 01; i < nbPts - 1; i++)
        {
            matTriCoeffs1[i] = 2 * (x[i + 1] - x[i - 1]) - deltasX[i - 1] * matTriCoeffs2[i - 1];
            matTriCoeffs2[i] = deltasX[i] / matTriCoeffs1[i];
            matTriCoeffs3[i] = (alpha[i] - deltasX[i - 1] * matTriCoeffs3[i - 1]) / matTriCoeffs1[i];
        }

        matTriCoeffs1[nbPts - 1] = 1;
        matTriCoeffs3[nbPts - 1] = 0;
        
        float[] c = new float[nbPts];
        c[nbPts - 1] = 0;
        
        for (int i = nbPts - 2; i >= 0; i--)
        {
            c[i] = matTriCoeffs3[i] - matTriCoeffs2[i] * c[i + 1];

        }

        float[] b = new float[nbPts - 1];
        float[] d = new float[nbPts - 1];
        float[,]coefficients=new float[nbPts-1,4];

        for (int i = 0; i < nbPts - 1; i++)
        {
            b[i] = (1 / deltasX[i]) * (y[i + 1] - y[i]) - (deltasX[i] / 3)
                * (2 * c[i] + c[i + 1]);
            d[i] = (c[i + 1] - c[i]) / (3 * deltasX[i]);
        }

        for (int i = 0; i < nbPts - 1; i++)
        {
            for (int j = 0; j < 4; j+=4)
            {
                coefficients[i, j] = y[i];
                coefficients[i, j + 1] = b[i];
                coefficients[i, j+2] = c[i];
                coefficients[i, j+3] = d[i];
                
            }
        }

    }





}
