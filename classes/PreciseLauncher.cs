using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using acolurk_base.helpers;
using Il2CppSystem;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Exception = System.Exception;
using Object = Il2CppSystem.Object;

namespace acolurk_base.classes;

public class PreciseLauncher
{
    public enum LauncherRole {Isolated = 0, Passer = 1, Goalie = 2}
    
    // fields
    private VelocityOptimization optimizer;
    private Vector3 start = new Vector3(0, 0, 40);
    private Vector3 target = new Vector3(0, 0, -40);
    private Vector3 launchVelocity;
    private float waitTime;
    private float spawnTimer;
    // private float launchTimer;
    private float interval;
    private float tof;
    private int precision;
    private bool isEnabled = false;
    private bool waiting = false;
    private LauncherRole launcherRole;
    private Puck ammo;
    private UIChat chat = NetworkBehaviourSingleton<UIChat>.instance;

    public PreciseLauncher(Vector3 start = default, Vector3 target = default, LauncherRole launcherRole = LauncherRole.Isolated, float waitTime = 1f, float interval = 5f, float tof = 3, int precision = 50)
    {
        this.start = start;
        this.target = target;
        this.launcherRole = launcherRole;
        this.waitTime = waitTime;
        this.interval = interval;
        this.precision = precision;
        this.tof = tof;

    }
    

    public void Toggle()
    {
        isEnabled = !isEnabled;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    public void Update()
    {
        if (isEnabled && !waiting) spawnTimer += Time.deltaTime;
        if (waiting)
        {
            if (optimizer.Step())
            {
                launchPuck();
            }
        }
        if (spawnTimer >= interval)
        {
            spawnTimer = 0;
            spawnPuck();
        }
    }

    private void launchPuck()
    {
        launchVelocity = optimizer.GetBestVelocity();
        ammo.Server_Unfreeze();
        ammo.Rigidbody.velocity = launchVelocity;
        waiting = false;
    }

    private void spawnPuck()
    {
        waiting = true;
        PuckManager pm = NetworkBehaviourSingleton<PuckManager>.instance;
        ammo = pm.Server_SpawnPuck(start, new Quaternion(0, 0, 0, 0), new Vector3(0, 0, 0));
        ammo.Server_Freeze();
        if (optimizer == null) optimizer = new VelocityOptimization(this.start, this.target, this.tof);
    }

    public void setStart(Vector3 start)
    {
        this.start = start;
        optimizer.SetStart(start);
    }

    public void setTarget(Vector3 target)
    {
        this.target = target;
        optimizer.SetTarget(target);
    }

    public void setTOF(float tof)
    {
        this.tof = tof;
        optimizer.SetTOF(tof);
    }

    public void setRole(LauncherRole role)
    {
        this.launcherRole = role;
    }
}