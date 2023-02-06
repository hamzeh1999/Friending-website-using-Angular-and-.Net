import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'enviroments/environment';
import { Photo } from '../_models/Photo';

@Injectable({
  providedIn: 'root'
})
export class UploadServiceService {
  baseUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) { }

  public uploadfile(file: File) {
    let formParams = new FormData();
    formParams.append('file', file);
    return this.httpClient.post<Photo>(this.baseUrl+'Users/add-photo', formParams);
  }
}
