<template>
<div>
  <div class="col-12">
    <div class="centerBlock">
      <div class="row">
        <div class="col">
          <img src="./assets/img/chromely.png" class="img-rounded" alt="Chromely Logo" width="200" height="200" style='margin-top: 20px;' />
        </div>
        <div class="col">
             <img src="./assets/img/logo.png" class="img-rounded" alt="Vue Logo" width="200" height="200" style='margin-top: 20px;' />
        </div>
      </div>

      <span class="text-primary text-center"><h2>chromely + vue</h2></span>
      <p class="text-muted text-center">Build .NET/.NET CORE HTML5 Desktop Apps</p>
    </div>
  </div>

  <div class="col-12" style='padding:20px;'>
    <div id="runButton" class="centerBlock">
      <form class="form-inline">
        <div class="form-group">
          <label for="info">RegisterAsyncJsObject/Http Demos:</label>
        </div>
        <router-link to="/demo"> <button id="buttonDemoRun" type="button" class="btn btn-primary" style='margin: 5px;'>Run</button></router-link>
      </form>
    </div>
  </div>

</div>
</template>

<script>
  export default {

  }
</script>