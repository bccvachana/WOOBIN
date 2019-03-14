import os
import cv2
import numpy as np
import tensorflow as tf
import sys
import socket

sys.path.append("..")

from utils import label_map_util
from utils import visualization_utils as vis_util

MODEL_NAME = 'inference_graph'

CWD_PATH = os.getcwd()

PATH_TO_CKPT = os.path.join(CWD_PATH,MODEL_NAME,'frozen_inference_graph.pb')

PATH_TO_LABELS = os.path.join(CWD_PATH,'training','labelmap.pbtxt')

NUM_CLASSES = 4

label_map = label_map_util.load_labelmap(PATH_TO_LABELS)
categories = label_map_util.convert_label_map_to_categories(label_map, max_num_classes=NUM_CLASSES, use_display_name=True)
category_index = label_map_util.create_category_index(categories)

detection_graph = tf.Graph()
with detection_graph.as_default():
    od_graph_def = tf.GraphDef()
    with tf.gfile.GFile(PATH_TO_CKPT, 'rb') as fid:
        serialized_graph = fid.read()
        od_graph_def.ParseFromString(serialized_graph)
        tf.import_graph_def(od_graph_def, name='')

    sess = tf.Session(graph=detection_graph)

image_tensor = detection_graph.get_tensor_by_name('image_tensor:0')

detection_boxes = detection_graph.get_tensor_by_name('detection_boxes:0')

detection_scores = detection_graph.get_tensor_by_name('detection_scores:0')
detection_classes = detection_graph.get_tensor_by_name('detection_classes:0')

num_detections = detection_graph.get_tensor_by_name('num_detections:0')

video = cv2.VideoCapture(0)
ret = video.set(3,1280720)
ret = video.set(4,287)

host, port = "127.0.0.1", 25001
data = "0.1,0.526,1.035,48.95,56.87"

#sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#sock.connect((host,port))

while(True):

    detect="0"
    ret, frame = video.read()
    frame_expanded = np.expand_dims(frame, axis=0)

    (boxes, scores, classes, num) = sess.run(
        [detection_boxes, detection_scores, detection_classes, num_detections],
        feed_dict={image_tensor: frame_expanded})

    vis_util.visualize_boxes_and_labels_on_image_array(
        frame,
        np.squeeze(boxes),
        np.squeeze(classes).astype(np.int32),
        np.squeeze(scores),
        category_index,
        use_normalized_coordinates=True,
        line_thickness=8,
        min_score_thresh=0.80)

    cv2.imshow('Object detector', frame) #close frame

    objects = np.where(classes[0] == 1)[0]
    if len(objects) > 0 and scores[0][objects][0] > 0.80:
        detect="1";
    objects = np.where(classes[0] == 2)[0]     
    if len(objects) > 0 and scores[0][objects][0] > 0.80:
        detect="2";
    objects = np.where(classes[0] == 3)[0]
    if len(objects) > 0 and scores[0][objects][0] > 0.80:
        detect="3";
    objects = np.where(classes[0] == 4)[0]
    if len(objects) > 0 and scores[0][objects][0] > 0.80:
        detect="4";

    ymin = boxes[0][0][0]*257
    xmin = boxes[0][0][1]*382
    ymax = boxes[0][0][2]*257
    xmax = boxes[0][0][3]*382

    data = detect+","+str(int(xmin))+","+str(int(ymin))+","+str(int(xmax))+","+str(int(ymax))
    print(data)

    #sock.sendall(data.encode("utf-8"))
    #data = sock.recv(1024).decode("utf-8")

    if cv2.waitKey(1) == ord('q'):
        break

video.release()
cv2.destroyAllWindows()

