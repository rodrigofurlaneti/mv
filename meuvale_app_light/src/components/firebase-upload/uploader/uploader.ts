import { Component, Inject, Input, Output, EventEmitter } from "@angular/core";
import * as firebase from 'firebase'
import { FirbaseUploadConfig, _FirbaseUploadConfig } from "../models";
import { Camera, CameraOptions, CameraPopoverOptions } from "@ionic-native/camera";
import { normalizeURL, Platform } from 'ionic-angular';

@Component({
    selector: 'firebase-uploader',
    templateUrl: 'uploader.html'
})
export class FirebaseUploaderComponent {

    @Input("file-name") private fileName: string;
    //caso tenha mais níveis separar com a '/' exe: images/teste. sem colocar uma barra no final... 
    //se for só um nível colocar somente o nome dele sem barra a frente ou atrás.
    @Input("path-file") private pathFile: string = 'images';
    @Output("uploadStart") private uploadStart: EventEmitter<void> = new EventEmitter();
    @Output("uploadComplete") private uploadComplete: EventEmitter<any> = new EventEmitter();
    @Output("imageCaptureError") private imageCaptureError: EventEmitter<any> = new EventEmitter();
    @Output("uploadError") private uploadError: EventEmitter<any> = new EventEmitter();

    selectedFile: any;
    mobile: boolean = (this.platform.is('ios') || this.platform.is('android'))

    constructor(
        @Inject(FirbaseUploadConfig) 
        private config: _FirbaseUploadConfig,
        private camera: Camera,
        private platform: Platform
    ) {
        if (!firebase.apps.length)
            firebase.initializeApp(this.config.firebase);
    }

    getPicture(inputType: number) {
        const options: CameraOptions = {
            quality: this.config.image.quality,
            targetHeight: this.config.image.height,
            targetWidth: this.config.image.width,
            destinationType: this.camera.DestinationType.DATA_URL,
            encodingType: this.camera.EncodingType.PNG,
            mediaType: this.camera.MediaType.PICTURE,
            sourceType: inputType ? inputType : 0,
            cameraDirection: this.camera.Direction.BACK,
            correctOrientation: true,
            allowEdit: true
        }

        this.fileName = this.fileName ? this.fileName : `${new Date().getDate().toString() + new Date().getMonth().toString() + new Date().getFullYear().toString()
            + new Date().getHours().toString() + new Date().getMinutes().toString() + new Date().getSeconds().toString()}.png`;

        this.camera.getPicture(options)
            .then((imageData) => {
                this.uploadStart.emit();
                this.upload(this.dataURItoBlob('data:image/png;base64,' + normalizeURL(imageData)));
            }, err => {
                console.log(err)
                this.imageCaptureError.emit(err);
            });
    }

    private dataURItoBlob(dataURI) {
        let binary = atob(dataURI.split(',')[1]);
        let array = [];
        for (let i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        return new Blob([new Uint8Array(array)], { type: 'image/png' });
    }

    private upload(photo: Blob) {
        if (photo) {
            var uploadTask = firebase.storage().ref().child(`${this.pathFile}/${this.fileName}`)
                .put(photo);
            uploadTask.then(
                snapshot => {
                    snapshot.ref.getDownloadURL().then((URL) => {
                        this.uploadComplete.emit({ URL: URL, FileName: this.fileName });
                    });
                },
                error => {
                    console.log(error)
                    this.uploadError.emit(error);
                }
            )
        }
    }

    public static deleteFromCloudStorage(folder, name) {

        // Get a reference to the storage service, which is used to create references in your storage bucket
        var storage = firebase.storage();

        // Create a storage reference from our storage service
        var storageRef = storage.ref();

        // Create a reference to the file to delete
        var desertRef = storageRef.child(`${folder}/${name}`);

        // Delete the file
        desertRef.delete().then(function () {
            // File deleted successfully
            console.log("Excluído com sucesso!");
        }).catch(function (error) {
            // Uh-oh, an error occurred!
            console.log("Erro: " + error);
        });
    }

    processFile(imageInput: any) {
        this.upload(imageInput.files[0]);
    }
}