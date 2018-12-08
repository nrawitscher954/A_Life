using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OrganizationalModel.Managers
{
    public class SimulationManager : MonoBehaviour
    {

        // singleton
        private static SimulationManager m_Instance = null;
        public static SimulationManager Get()
        {
            if (m_Instance == null)
                m_Instance = (SimulationManager)FindObjectOfType(typeof(SimulationManager));
            return m_Instance;
        }

        public bool runSimulation;
        public bool exportTrailPaths;
        public GameObject placeHolderPrefab;
        public GameObject agentPrefab;

        public string individualOrganizationFilePath;
        public string CommunicationTypeFilePath;
        public string DirectionalityRuleFilePath;
        public string NeighbourCountFilePath;
        public string EnergyLevelsFilePath;
        public string ClusterOrganizationFilePath;
        public string DensityThresholdFilePath;
        public string ScalarFieldValuesFilePath;
        public string PeopleFilePath;
        public string PixelArrayFilePath;
        public string PixelDataClusterAgentsPath;
        public string PixelDataMobileAgentsPath;
        public string WindAnalysisFilePath;
        public string ImportToUnityInterpolatedFieldFilePath;
        public string ImportToUnityClusterStartingPositionsFilePath;
        public string ImportToUnityDomainsFilePath;
        

        public bool displayColorByState;
        public bool displayColorbyEnergy;
        public bool displayColorbyDisplacement;
        public bool displayColorMonochrome;
        public bool displayColorByNeighbours;
        public bool displayColorByComunication;
        public bool GPUInstancing;
        public bool is2D;
        public bool is3D;
        public bool ScalarField3d;
        public bool ScalarField2d;

        public bool gridDistribution_6Neighbours;
        public bool _12Neighbours;

        public bool addFixedJoints;
        public bool addRigidBodyCollider;

        public bool radialScalarField;
        public bool interpolatedScalarField;
        public bool singleRule;
        public bool multipleRules;

        public Color startColorEnergy;
        public Color endColorEnergy;

        public Color startColorDisplacement;
        public Color endColorDisplacement;

        public Color monochromeColor;

        public Color startColorNeighbours;
        public Color endColorNeighbours;

        public Color colorLine;
        public Color colorBranch;
        public Color colorStar;

    }
}
