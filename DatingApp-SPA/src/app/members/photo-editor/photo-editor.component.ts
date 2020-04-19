import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { tokenGetter } from 'src/app/_services/token-storage.service';
import { PhotoService } from 'src/app/_services/photo.service';
import { AlertService } from 'src/app/_services/alert.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() mainPhotoSet = new EventEmitter<Photo>();

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;

  constructor(private photoService: PhotoService, private alert: AlertService) {
    this.uploader = new FileUploader({
      url: '',
      disableMultipart: true, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      formatDataFunctionIsAsync: true,
      formatDataFunction: async (item) => {
        return new Promise((resolve, reject) => {
          resolve({
            name: item._file.name,
            length: item._file.size,
            contentType: item._file.type,
            date: new Date(),
          });
        });
      },
    });

    this.hasBaseDropZoneOver = false;
    this.hasAnotherDropZoneOver = false;

    this.response = '';

    this.uploader.response.subscribe((res) => (this.response = res));
  }

  setMain(photo: Photo): void {
    this.photoService.setMain(photo.id).subscribe(response => {
      this.photos.find(x => x.isMain).isMain = false;
      photo.isMain = true;
      this.mainPhotoSet.emit(photo);
    },
    error => {
      this.alert.error(error);
    });
  }

  ngOnInit(): void {
    this._initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  private _initializeUploader(): void {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'photos/upload',
      authToken: 'Bearer ' + tokenGetter(),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.photos.push(photo);
      }
    };
  }
}
