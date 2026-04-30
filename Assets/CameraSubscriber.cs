using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Nav;

public class CameraOdometrySubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/lidar_ground_truth";

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<OdometryMsg>(topicName, OdometryCallback);

        Debug.Log($"[CameraOdometrySubscriber] Subscribed to topic: {topicName}");
    }

    void OdometryCallback(OdometryMsg msg)
    {
        Vector3 position = new Vector3(
            -(float)msg.pose.pose.position.y, // ROS left -> Unity right
            (float)msg.pose.pose.position.z,  // ROS up   -> Unity up
            (float)msg.pose.pose.position.x   // ROS fwd  -> Unity fwd
        );

        // Quaternion rotation = new Quaternion(
        //     (float)msg.pose.pose.orientation.x,
        //     (float)msg.pose.pose.orientation.z,
        //     (float)msg.pose.pose.orientation.y,
        //     (float)msg.pose.pose.orientation.w
        // );

        // Update camera
        transform.position = position;
        // transform.rotation = rotation;

        // Debug logging
        Debug.Log($"[CameraOdometrySubscriber] Received odometry message:");
        Debug.Log($"Position -> x: {position.x:F2}, y: {position.y:F2}, z: {position.z:F2}");
        // Debug.Log($"Rotation -> x: {rotation.x:F2}, y: {rotation.y:F2}, z: {rotation.z:F2}, w: {rotation.w:F2}");
    }
}